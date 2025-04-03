using AucX.DataAccess.Context;
using AucX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AucX.DataAccess.Repositories
{
public class ColorShopRepository : IColorShopRepository
    {
        private readonly AppDbContext _context;
        private readonly AppSettings _appSettings;

        public ColorShopRepository(AppDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<List<ColorShop>> GetAllColorsAsync()
        {
            return _appSettings.ColorShop.Select(c => new ColorShop
            {
                Id = c.Id,
                Name = c.Name,
                ColorCode = c.ColorCode,
                Price = c.Price
            }).ToList();
        }

        public async Task<ColorShop> GetColorByIdAsync(int id)
        {
            var color = _appSettings.ColorShop.FirstOrDefault(c => c.Id == id);
            return new ColorShop
            {
                Id = color.Id,
                Name = color.Name,
                ColorCode = color.ColorCode,
                Price = color.Price
            };
        }
    }
}
