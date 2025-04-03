using AucX.DataAccess.Context;
using AucX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AucX.DataAccess.Repositories
{
    public class PixelArtRepository : IPixelArtRepository
    {
        private readonly AppDbContext _context;

        public PixelArtRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PixelArt> CreatePixelArtAsync(PixelArt pixelArt)
        {
            _context.PixelArts.Add(pixelArt);
            await _context.SaveChangesAsync();
            return pixelArt;
        }

        public async Task<PixelArt> GetPixelArtByIdAsync(int id)
        {
            return await _context.PixelArts.Include(p => p.Colors).FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
