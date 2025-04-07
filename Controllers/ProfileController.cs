using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AucX.Domain.Entities;
using AucX.DataAccess.Context;
using AucX.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AucX.WebUI.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserCanvasUpgradeService _upgradeService;
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _context;

        public ProfileController(
            UserManager<AppUser> userManager,
            IUserCanvasUpgradeService upgradeService,
            IOptions<AppSettings> appSettings,
            AppDbContext context)
        {
            _userManager = userManager;
            _upgradeService = upgradeService;
            _appSettings = appSettings.Value;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var canvasUpgrade = await _context.UserCanvasUpgrades.FirstOrDefaultAsync(u => u.UserId == user.Id);
            ViewBag.CanvasUpgrade = canvasUpgrade;

            ViewBag.CanvasUpgradePrice = _appSettings.CanvasUpgradePrice;
            ViewBag.ColorPurchasePrice = _appSettings.ColorPurchasePrice;

            int currentWidth = canvasUpgrade != null ? canvasUpgrade.MaxWidth : _appSettings.InitialCanvasWidth;
            int currentHeight = canvasUpgrade != null ? canvasUpgrade.MaxHeight : _appSettings.InitialCanvasHeight;

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

            if (user.Balance < _appSettings.CanvasUpgradePrice)
            {
                TempData["Error"] = "Недостаточно средств для покупки улучшения.";
                return RedirectToAction("Index");
            }

            // Списываем средства
            user.Balance -= _appSettings.CanvasUpgradePrice;
            await _userManager.UpdateAsync(user);

            await _upgradeService.UpgradeCanvasAsync(user.Id);

            TempData["Success"] = "Улучшение успешно куплено!";
            return RedirectToAction("Index");
        }
    }
}
