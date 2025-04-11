using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AucX.Domain.Entities;

namespace AucX.WebUI.Controllers
{
    [Authorize]
    public class ClickerController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ClickerController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Click()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            
            if (await _userManager.IsInRoleAsync(user, "Admin")) {
                user.Balance += 1000;
            } else {
                user.Balance += 1;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return StatusCode(500, "Не удалось обновить баланс");
            }

            return Ok(new { balance = user.Balance });
        }

        [HttpGet]
        public async Task<IActionResult> GetBalance()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(new { balance = user.Balance });
        }
    }
}
