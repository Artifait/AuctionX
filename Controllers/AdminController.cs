using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AucX.Domain.Entities;
using AucX.DataAccess.Repositories;

namespace AucX.WebUI.Controllers
{
[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        // Просмотр списка администраторов
        public IActionResult AdminsList()
        {
            var admins = _userManager.GetUsersInRoleAsync("Admin").Result;
            return View(admins);
        }

        // GET: /Admin/AddAdmin
        public IActionResult AddAdmin()
        {
            return View();
        }

        // POST: /Admin/AddAdmin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdmin(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, "Admin");
                if (result.Succeeded)
                {
                    return RedirectToAction("AdminsList");
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка при добавлении администратора");
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }
            return View();
        }

        // POST: /Admin/RemoveAdmin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAdmin(string userId, [FromServices] IConfiguration config)
        {
            var adminEmail = config.GetSection("AppSettings")["AdminEmail"]!;
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Проверка: нельзя удалить администратора с почтой, указанной в appsettings (главного администратора)
                if (string.Equals(user.Email, adminEmail, StringComparison.OrdinalIgnoreCase))
                {
                    TempData["Error"] = "Нельзя удалить главного администратора.";
                    return RedirectToAction("AdminsList");
                }
                
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, "Admin");
                    if (result.Succeeded)
                    {
                        TempData["Message"] = "Администратор успешно удален.";
                        return RedirectToAction("AdminsList");
                    }
                }
                TempData["Error"] = "Ошибка при удалении администратора.";
            }
            else
            {
                TempData["Error"] = "Пользователь не найден.";
            }
            return RedirectToAction("AdminsList");
        }

    }
}
