using AucX.Application.DTOs;
using AucX.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AucX.Presentation.Controllers
{
public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            await _authService.RegisterUserAsync(model);
            return RedirectToAction("Index", "Home");
        }
    }
}
