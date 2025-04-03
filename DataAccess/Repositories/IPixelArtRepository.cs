using AucX.Domain.Entities;
using System.Threading.Tasks;

namespace AucX.DataAccess.Repositories
{
    public interface IPixelArtRepository
    {
        Task<PixelArt> CreatePixelArtAsync(PixelArt pixelArt);
        Task<PixelArt> GetPixelArtByIdAsync(int id);
    }
}
