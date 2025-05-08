using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AucX.WebUI.Infrastructure;
using AucX.WebUI.Models;
using System.Security.Claims;
using AucX.WebUI.DTO;
using Microsoft.AspNetCore.Identity;
using AucX.Domain.Entities;
using AucX.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace AucX.WebUI.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IAuctionService _auctionService;
        private readonly IBalanceService _balanceService;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public AuctionController(
            IAuctionService auctionService,
            IBalanceService balanceService,
            UserManager<AppUser> userManager,
            AppDbContext context)
        {
            _auctionService = auctionService;
            _balanceService = balanceService;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("auction")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 12)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var lots = await _auctionService.GetActiveAuctionsAsync(page, pageSize);
            var totalCount = await _context.AuctionLots
                .CountAsync(al => al.Status == AuctionLotStatus.Active);

            var availableBalance = 0m;
            var frozenBalance = 0m;

            if (User.Identity.IsAuthenticated)
            {
                availableBalance = await _balanceService.GetAvailableBalanceAsync(userId);
                frozenBalance = await _balanceService.GetFrozenBalanceAsync(userId);
            }

            var view = View(new AuctionIndexViewModel
            {
                Lots = lots.Select(l => new AuctionLotDto
                {
                    Id = l.Id,
                    CanvasItem = new CanvasItemDto
                    {
                        Id = l.CanvasItem.Id,
                        Name = l.CanvasItem.Name,
                        Width = l.CanvasItem.Width,
                        Height = l.CanvasItem.Height,
                        PixelData = l.CanvasItem.PixelData
                    },
                    CurrentBid = (l.Bids.Count == 0) ? 0 : l.Bids.Max(b => b.Amount),
                    MinimumPrice = l.MinimumPrice,
                    EndTime = l.EndTime
                }).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                UserBalance = availableBalance,
                FrozenBalance = frozenBalance
            });
            return view;
        }

        [HttpGet("auction/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var lotDetails = await _auctionService.GetAuctionDetailsAsync(id);
                if (lotDetails?.Lot == null) return NotFound();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                var availableBalance = User.Identity.IsAuthenticated
                    ? await _balanceService.GetAvailableBalanceAsync(userId)
                    : 0;

                var frozenBalance = User.Identity.IsAuthenticated
                    ? await _balanceService.GetFrozenBalanceAsync(userId)
                    : 0;

                return View(new AuctionLotViewModel
                {
                    Id = lotDetails.Lot.Id,
                    CanvasItem = new CanvasItemDto
                    {
                        Id = lotDetails.Lot.CanvasItem.Id,
                        Name = lotDetails.Lot.CanvasItem.Name,
                        Width = lotDetails.Lot.CanvasItem.Width,
                        Height = lotDetails.Lot.CanvasItem.Height,
                        PixelData = lotDetails.Lot.CanvasItem.PixelData
                    },
                    CurrentBid = lotDetails.CurrentPrice,
                    MinimumPrice = lotDetails.Lot.MinimumPrice,
                    StartingPrice = lotDetails.Lot.StartingPrice,
                    MinBidIncrement = lotDetails.Lot.MinBidIncrement,
                    EndTime = lotDetails.Lot.EndTime,
                    TimeLeft = lotDetails.TimeLeft,
                    Bids = lotDetails.Lot.Bids
                        .OrderByDescending(b => b.BidTime)
                        .Select(b => new BidDto
                        {
                            UserName = b.User.UserName,
                            Amount = b.Amount,
                            BidTime = b.BidTime
                        }).ToList(),
                    IsOwner = lotDetails.Lot.CanvasItem.UserId == userId,
                    AvailableBalance = availableBalance,
                    FrozenBalance = frozenBalance
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("auction/{id}/bid")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceBid(int id, [FromForm] decimal amount)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var bid = await _auctionService.PlaceBidAsync(id, userId, amount);
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Details", new { id });
            }
        }

        [HttpPost("auction/{id}/cancel")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAuction(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _auctionService.CancelAuctionAsync(id, userId);
                return RedirectToAction("Index", "Profile");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Details", new { id });
            }
        }
    }
}
