using AucX.DataAccess.Context;
using AucX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AucX.DataAccess.Repositories;


// public class AuctionLotRepository : IAuctionLotRepository
// {
//     private readonly AppDbContext _context;

//     public AuctionLotRepository(AppDbContext context)
//     {
//         _context = context;
//     }

//     public async Task<IEnumerable<AuctionLot>> GetAllAuctionLotsAsync()
//     {
//         return await _context.AuctionLots
//             .Include(a => a.GameItem)
//             .ToListAsync();
//     }

//     public async Task<AuctionLot> GetAuctionLotByIdAsync(int id)
//     {
//         return await _context.AuctionLots
//             .Include(a => a.GameItem)
//             .FirstOrDefaultAsync(a => a.Id == id)!;
//     }

//     public async Task AddBidAsync(int lotId, decimal bidAmount)
//     {
//         var lot = await _context.AuctionLots.FindAsync(lotId);
//         if (lot != null && bidAmount >= lot.CurrentBid + lot.MinBidIncrement)
//         {
//             lot.CurrentBid = bidAmount;
//             await _context.SaveChangesAsync();
//         }
//     }
// }