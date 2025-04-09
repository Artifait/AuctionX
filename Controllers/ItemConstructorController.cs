using AucX.DataAccess.Repositories;
using AucX.Domain.Entities;
using AucX.WebUI.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AucX.WebUI.Controllers
{
    [Authorize]
    [Route("ItemConstructor")]
    public class ItemConstructorController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICanvasItemRepository _canvasItemRepository;

        public ItemConstructorController(
            UserManager<AppUser> userManager,
            ICanvasItemRepository canvasItemRepository)
        {
            _userManager = userManager;
            _canvasItemRepository = canvasItemRepository;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("CreateCanvas")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCanvas([FromBody] CanvasCreateModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized();

                var existingCanvas = await _canvasItemRepository.GetCanvasItemByUserAndNameAsync(user.Id, model.Name);
                if (existingCanvas != null)
                {
                    ModelState.AddModelError("", "Холст с таким именем уже существует");
                    return View("Index", model);
                }

                var canvasItem = new CanvasItem
                {
                    Name = model.Name,
                    ColorCodes = model.ColorCodes,
                    Width = model.Width,
                    Height = model.Height,
                    UserId = user.Id
                };

                await _canvasItemRepository.AddCanvasItemAsync(canvasItem);
                await _canvasItemRepository.SaveAsync();

                return Json(new { success = true, message = "Холст успешно сохранён!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }

    public class CanvasCreateModel
    {
        public string Name { get; set; } = null!;
        public string ColorCodes { get; set; } = null!;
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
