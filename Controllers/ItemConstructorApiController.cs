using AucX.DataAccess.Repositories;
using AucX.Domain.Entities;
using AucX.WebUI.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AucX.WebUI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemConstructorApiController : ControllerBase
    {
        private readonly IAppSettingsService _appSettingsService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserColorRepository _userColorRepository;

        public ItemConstructorApiController(
            IAppSettingsService appSettingsService,
            UserManager<AppUser> userManager,
            IUserColorRepository userColorRepository)
        {
            _appSettingsService = appSettingsService;
            _userManager = userManager;
            _userColorRepository = userColorRepository;
        }

        [HttpGet("user-info")]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var colorCollections = _appSettingsService.GetColorCollections();
            var availableColors = colorCollections.FirstOrDefault()?.Default ?? new List<Color>();

            var purchasedColors = await _userColorRepository.GetPurchasedColorsAsync(user.Id);

            var result = new
            {
                availableColors,
                purchasedColors,
                maxCanvasWidth = user.MaxCanvasWidth,
                maxCanvasHeight = user.MaxCanvasHeight,
                minCanvasHeight = _appSettingsService.GetInitialCanvasSize().height,
                minCanvasWidth = _appSettingsService.GetInitialCanvasSize().width,
            };

            return Ok(result);
        }
    }
}