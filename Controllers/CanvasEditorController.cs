// Controllers/CanvasEditorController.cs
using AucX.DataAccess.Repositories;
using AucX.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AucX.WebUI.Controllers
{
    [Authorize]
    public class CanvasEditorController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICanvasItemRepository _canvasRepo;
        private readonly IUserColorRepository _colorRepo;

        public CanvasEditorController(
            UserManager<AppUser> userManager,
            ICanvasItemRepository canvasRepo,
            IUserColorRepository colorRepo)
        {
            _userManager = userManager;
            _canvasRepo = canvasRepo;
            _colorRepo = colorRepo;
        }

         [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var colors = await _colorRepo.GetPurchasedColorsAsync(user.Id);
            
            return View(new CanvasEditorViewModel
            {
                UserColors = colors.ToList(),
                Tools = new List<string> { "brush", "Заливка" },
                MaxWidth = user.MaxCanvasWidth,
                MaxHeight = user.MaxCanvasHeight
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCanvas([FromBody] CanvasSaveRequest request)
        {
            // Валидация модели
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            if (request.Width > user.MaxCanvasWidth || request.Height > user.MaxCanvasHeight)
                return BadRequest("Превышен максимальный размер холста");

            try
            {
                var canvasItem = new CanvasItem
                {
                    UserId = user.Id,
                    Name = request.Name.Trim(),
                    Width = request.Width,
                    Height = request.Height,
                    PixelData = string.Join(';', request.Pixels),
                    CreatedAt = DateTime.UtcNow
                };

                await _canvasRepo.AddCanvasItemAsync(canvasItem);
                await _canvasRepo.SaveAsync();
                
                return Ok(new { canvasItem.Id });
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка сохранения: {ex.Message}");
            }
        }

        private string ConvertToPixelData(string[] pixels, int width)
        {
            if (pixels.Length != width * width)
                throw new ArgumentException("Некорректные размеры холста");

            return string.Join(';', 
                pixels.Select((p, i) => (i + 1) % width == 0 ? p + ";" : p)
                    .Select(s => s.TrimEnd(';'))
            );
        }
    }

    public class CanvasEditorViewModel
    {
        public string CanvasName { get; set; }
        public List<UserColor> UserColors { get; set; }
        public List<string> Tools { get; set; }
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }
    }

    public class CanvasSaveRequest
    {
        public int? CanvasId { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string[] Pixels { get; set; }
    }
}