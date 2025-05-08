using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AucX.Domain.Entities;
using AucX.DataAccess.Repositories;
using AucX.WebUI.Infrastructure;
using AucX.WebUI.Models;

namespace AucX.WebUI.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICanvasItemRepository _canvasRepo;
        private readonly IAuctionService _auctionService;
        private readonly IBalanceService _balanceService;

        public ProfileController(
            UserManager<AppUser> userManager,
            ICanvasItemRepository canvasRepo,
            IAuctionService auctionService,
            IBalanceService balanceService)
        {
            _userManager = userManager;
            _canvasRepo = canvasRepo;
            _auctionService = auctionService;
            _balanceService = balanceService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var activeLots = await _auctionService.GetUserActiveLotsAsync(user!.Id);
            var userArts = await _canvasRepo.GetUserCanvasItemsAsync(user.Id);

            var availableBalance = await _balanceService.GetAvailableBalanceAsync(user.Id);
            var frozenBalance = await _balanceService.GetFrozenBalanceAsync(user.Id);
            
            return View(new ProfileViewModel
            {
                UserName = user.UserName!,
                Email = user.Email!,
                AvailableBalance = availableBalance,
                FrozenBalance = frozenBalance,
                UserCanvasItems = userArts.Select(a => new CanvasItemDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    PixelData = a.PixelData,
                    Width = a.Width,
                    Height = a.Height,
                }).ToList(),
                AuctionLots = activeLots.Select(l => new AuctionLotDto
                {
                    Id = l.Id,
                    CanvasItem = new CanvasItemDto
                    {
                        Id = l.CanvasItem.Id,
                        Name = l.CanvasItem.Name,
                        PixelData = l.CanvasItem.PixelData,
                        Width = l.CanvasItem.Width,
                        Height = l.CanvasItem.Height,
                    },
                    CurrentBid = l.Bids.Max(b => b.Amount),
                    MinimumPrice = l.MinimumPrice,
                    EndTime = l.EndTime
                }).ToList()
            });
        }
    }
}
