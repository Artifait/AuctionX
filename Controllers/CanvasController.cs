using AucX.DataAccess.Repositories;
using AucX.Domain.Entities;
using AucX.WebUI.Infrastructure;
using AucX.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AucX.WebUI.Controllers
{
    [Authorize]
    public class CanvasController : Controller
    {
        private readonly ICanvasItemRepository _canvasRepo;
        private readonly IUserColorRepository _colorRepo;
        private readonly IAppSettingsService _settings;
        private readonly UserManager<AppUser> _userManager;

        public CanvasController(
            ICanvasItemRepository canvasRepo,
            IUserColorRepository colorRepo,
            IAppSettingsService settings,
            UserManager<AppUser> userManager)
        {
            _canvasRepo = canvasRepo;
            _colorRepo = colorRepo;
            _settings = settings;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await GetCurrentUser();
            return View(await CreateViewModelAsync(user));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CanvasViewModel model)
        {
            var user = await GetCurrentUser();
            if (!ModelState.IsValid)
                return View(await CreateViewModelAsync(user, model));

            var canvasItem = new CanvasItem
            {
                Name = model.Name,
                Width = model.Width,
                Height = model.Height,
                PixelData = Request.Form["pixelData"],
                UserId = user.Id
            };

            try
            {
                await _canvasRepo.AddCanvasItemAsync(canvasItem);
                await _canvasRepo.SaveAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (InvalidOperationException ex)
            {
                model.ErrorMessage = ex.Message;
                return View(await CreateViewModelAsync(user, model));
            }
        }

        private async Task<AppUser> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(User);
        }

        private async Task SaveCanvasItem(CanvasItem item)
        {
            await _canvasRepo.AddCanvasItemAsync(item);
            await _canvasRepo.SaveAsync();
        }

        private async Task<List<string>> GetUserColorsAsync(AppUser user)
        {
            var purchased = (await _colorRepo.GetPurchasedColorsAsync(user.Id))
                .Select(c => c.ColorCode);
            var collections = _settings.GetColorCollections();
            
            var defaults = collections
                .SelectMany(c => c.Default.Select(d => d.ColorCode));
            
            return purchased.Concat(defaults).Distinct().ToList();
        }

        private async Task<CanvasViewModel> CreateViewModelAsync(AppUser user, CanvasViewModel? model = null)
        {
            var vm = model ?? new CanvasViewModel();
            var settings = _settings.GetMaxCanvasSize();
            
            vm.MaxWidth = Math.Min(settings.maxWidth, user.MaxCanvasWidth);
            vm.MaxHeight = Math.Min(settings.maxHeight, user.MaxCanvasHeight);
            vm.AvailableColors = await GetUserColorsAsync(user);
            
            return vm;
        }
    }
}
