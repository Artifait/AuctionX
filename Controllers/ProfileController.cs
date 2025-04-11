using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AucX.Domain.Entities;
using AucX.DataAccess.Context;
using AucX.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AucX.WebUI.Infrastructure;

namespace AucX.WebUI.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IAppSettingsService _appSettingsService;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public ProfileController(
            UserManager<AppUser> userManager,
            IAppSettingsService appSettingsService,
            AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
            _appSettingsService = appSettingsService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            int currentWidth = 10;
            int currentHeight = 10;

            ViewBag.CurrentCanvasWidth = currentWidth;
            ViewBag.CurrentCanvasHeight = currentHeight;

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpgradeCanvas()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            if (user.Balance < _appSettingsService.GetCanvasUpgradePrice())
            {
                TempData["Error"] = "Недостаточно средств для покупки улучшения.";
                return RedirectToAction("Index");
            }

            // Списываем средства
            user.Balance -= _appSettingsService.GetCanvasUpgradePrice();
            await _userManager.UpdateAsync(user);

            TempData["Success"] = "Улучшение успешно куплено!";
            return RedirectToAction("Index");
        }
    }
}
