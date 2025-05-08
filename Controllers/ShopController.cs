// Controllers/ShopController.cs
using AucX.DataAccess.Context;
using AucX.Domain.Entities;
using AucX.WebUI.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AucX.WebUI.Controllers
{
    [Authorize]
    public class ShopController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppSettingsService _appSettings;
        private readonly AppDbContext _context;

        public ShopController(
            UserManager<AppUser> userManager,
            IAppSettingsService appSettings,
            AppDbContext context)
        {
            _userManager = userManager;
            _appSettings = appSettings;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var colors = await GetShopColorsWithOwnership(user);
            var (maxWidth, maxHeight) = _appSettings.GetMaxCanvasSize();

            var model = new ShopViewModel
            {
                Balance = user.Balance,
                Colors = colors,
                CurrentWidth = user.MaxCanvasWidth,
                CurrentHeight = user.MaxCanvasHeight,
                UpgradePrice = _appSettings.GetCanvasUpgradePrice(),
                MaxWidth = maxWidth,
                MaxHeight = maxHeight
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BuyColor(string category, string colorName)
        {
            var user = await _userManager.GetUserAsync(User);
            var color = _appSettings.GetColorCollections()
                .SelectMany(c => c.GetColors())
                .FirstOrDefault(c => c.Category == category && c.Name == colorName);

            if (color == null) return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (user.Balance < color.Price)
                {
                    TempData["Error"] = "Недостаточно средств";
                    return RedirectToAction("Index");
                }

                user.Balance -= color.Price;
                await _userManager.UpdateAsync(user);

                _context.UserColors.Add(new UserColor
                {
                    UserId = user.Id,
                    Name = color.Name,
                    ColorCode = color.ColorCode,
                    Price = color.Price,
                    Category = color.Category
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = $"Цвет {color.Name} куплен!";
            }
            catch
            {
                await transaction.RollbackAsync();
                TempData["Error"] = "Ошибка при покупке";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpgradeCanvas()
        {
            var user = await _userManager.GetUserAsync(User);
            var price = _appSettings.GetCanvasUpgradePrice();
            var (maxWidth, maxHeight) = _appSettings.GetMaxCanvasSize();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (user!.Balance < price || 
                    user.MaxCanvasWidth >= maxWidth || 
                    user.MaxCanvasHeight >= maxHeight)
                {
                    TempData["Error"] = "Нельзя улучшить";
                    return RedirectToAction("Index");
                }

                user.Balance -= price;
                user.MaxCanvasWidth = Math.Min(user.MaxCanvasWidth + 1, maxWidth);
                user.MaxCanvasHeight = Math.Min(user.MaxCanvasHeight + 1, maxHeight);

                await _userManager.UpdateAsync(user);
                await transaction.CommitAsync();

                TempData["Success"] = "Холст улучшен!";
            }
            catch
            {
                await transaction.RollbackAsync();
                TempData["Error"] = "Ошибка улучшения";
            }

            return RedirectToAction("Index");
        }

        private async Task<List<ShopColor>> GetShopColorsWithOwnership(AppUser user)
        {
            var purchased = await _context.UserColors
                .Where(uc => uc.UserId == user.Id)
                .Select(uc => new { uc.Category, uc.Name })
                .ToListAsync();

            return _appSettings.GetColorCollections()
                .SelectMany(c => c.GetColors())
                .Select(c => new ShopColor(c)
                {
                    IsOwned = purchased.Any(p => p.Category == c.Category && p.Name == c.Name)
                })
                .ToList();
        }
    }

    public class ShopViewModel
    {
        public decimal Balance { get; set; }
        public List<ShopColor> Colors { get; set; }
        public int CurrentWidth { get; set; }
        public int CurrentHeight { get; set; }
        public int UpgradePrice { get; set; }
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }
    }

    public class ShopColor
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; }
        public int Price { get; set; }
        public bool IsOwned { get; set; }

        public ShopColor() { }

        public ShopColor(Color color)
        {
            Category = color.Category;
            Name = color.Name;
            ColorCode = color.ColorCode;
            Price = color.Price;
        }
    }
}