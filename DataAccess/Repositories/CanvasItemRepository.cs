using AucX.DataAccess.Context;
using AucX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AucX.DataAccess.Repositories
{
    public interface ICanvasItemRepository
    {
        Task<CanvasItem?> GetCanvasItemAsync(int id);
        Task<CanvasItem?> GetCanvasItemByUserAndNameAsync(string userId, string name);
        Task AddCanvasItemAsync(CanvasItem canvasItem);
        Task SaveAsync();
    }

    public class CanvasItemRepository : ICanvasItemRepository
    {
        private readonly AppDbContext _context;

        public CanvasItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CanvasItem?> GetCanvasItemAsync(int id)
        {
            return await _context.CanvasItems.FindAsync(id);
        }

        public async Task<CanvasItem?> GetCanvasItemByUserAndNameAsync(string userId, string name)
        {
            return await _context.CanvasItems
                .Where(c => c.UserId == userId && c.Name == name)
                .FirstOrDefaultAsync();
        }

        public async Task AddCanvasItemAsync(CanvasItem canvasItem)
        {
            bool exists = await _context.CanvasItems
                .AnyAsync(c => c.Width == canvasItem.Width 
                            && c.Height == canvasItem.Height
                            && c.PixelData == canvasItem.PixelData);

            if (exists) throw new InvalidOperationException("Холст уже существует");
            
            _context.CanvasItems.Add(canvasItem);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
