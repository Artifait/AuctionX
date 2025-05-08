using AucX.DataAccess.Repositories;
using AucX.Domain.Entities;
using AucX.WebUI.Infrastructure;
using AucX.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AucX.WebUI.Controllers
{
    public class CanvasController : Controller
    {
        private readonly ICanvasItemRepository _canvasRepo;
        private readonly IAuctionService _auctionService;
        private readonly UserManager<AppUser> _userManager;

        public CanvasController(
            ICanvasItemRepository canvasRepo,
            IAuctionService auctionService,
            UserManager<AppUser> userManager)
        {
            _canvasRepo = canvasRepo;
            _auctionService = auctionService;
            _userManager = userManager;
        }

        [HttpGet("Canvas/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var canvasItem = await _canvasRepo.GetCanvasItemAsync(id);
            if (canvasItem == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            var isOwner = userId == canvasItem.UserId;
            var canBeAuctioned = isOwner && !await _canvasRepo.IsItemInAuctionAsync(id);

            var vm = new CanvasItemDetailsViewModel()
            {
                Id = canvasItem.Id,
                Name = canvasItem.Name,
                PixelData = canvasItem.PixelData,
                Width = canvasItem.Width,
                Height = canvasItem.Height,
                AuthorId = canvasItem.UserId,
                CreatedAt = canvasItem.CreatedAt,
                IsOwner = isOwner,
                CanBeAuctioned = canBeAuctioned,
                AuctionForm = canBeAuctioned ? new CreateAuctionViewModel
                {
                    CanvasItemId = canvasItem.Id
                } : null
            };
            return View(vm);
        }

        [HttpPost("Canvas/{id}")]
        [Authorize]
        public async Task<IActionResult> Details(
            int id,
            [Bind(Prefix = "AuctionForm")] CreateAuctionViewModel model)
        {
            var canvasItem = await _canvasRepo.GetCanvasItemAsync(id);
            if (canvasItem == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (canvasItem.UserId != userId) return Forbid();

            if (!ModelState.IsValid)
            {
                // восстанавливаем ViewModel для вывода ошибок
                var vm = new CanvasItemDetailsViewModel()
                {
                    Id = canvasItem.Id,
                    Name = canvasItem.Name,
                    PixelData = canvasItem.PixelData,
                    Width = canvasItem.Width,
                    Height = canvasItem.Height,
                    AuthorId = canvasItem.UserId,
                    CreatedAt = canvasItem.CreatedAt,
                    IsOwner = true,
                    CanBeAuctioned = true,
                    AuctionForm = model
                };
                return View(vm);
            }

            await _auctionService.CreateAuctionLotAsync(
                model.CanvasItemId,
                model.MinimumPrice,
                model.StartingPrice,
                model.MinBidIncrement,
                model.EndTime);

            TempData["AuctionSuccess"] = "Аукцион успешно создан!";
            return RedirectToAction(nameof(Details), new { id });
        }

    }
}
