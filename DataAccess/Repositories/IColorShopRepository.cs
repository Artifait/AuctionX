
using AucX.Domain.Entities;

namespace AucX.DataAccess.Repositories
{
    public interface IColorShopRepository
    {
        Task<List<ColorShop>> GetAllColorsAsync();
        Task<ColorShop> GetColorByIdAsync(int id);
    }
}
