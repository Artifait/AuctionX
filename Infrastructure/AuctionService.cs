using System.Transactions;
using AucX.DataAccess.Context;
using AucX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using AucX.WebUI.DTO;

namespace AucX.WebUI.Infrastructure;

public interface IAuctionService
{
    Task<decimal> GetAvailableBalanceAsync(string userId);
    Task<AuctionLot> CreateAuctionLotAsync(int canvasItemId, decimal minPrice, decimal startPrice, decimal minIncrement, DateTime endTime);
    Task<Bid> PlaceBidAsync(int lotId, string userId, decimal amount);
    Task CancelBidAsync(int bidId, string userId);
    Task CompleteAuctionAsync(int lotId);
    Task<IEnumerable<AuctionLot>> GetUserActiveLotsAsync(string userId);

    Task CancelAuctionAsync(int lotId, string userId);
    Task<IEnumerable<AuctionLot>> GetActiveAuctionsAsync(int page, int pageSize);
    Task<AuctionLotDetailsDto> GetAuctionDetailsAsync(int lotId);
}

public class AuctionService : IAuctionService
{
    private readonly AppDbContext _context;
    private readonly IBalanceService _balanceService;

    public AuctionService(AppDbContext context, IBalanceService balanceService)
    {
        _context = context;
        _balanceService = balanceService;
    }

    public async Task<decimal> GetAvailableBalanceAsync(string userId)
    {
        return await _balanceService.GetAvailableBalanceAsync(userId);
    }

    public async Task<AuctionLot> CreateAuctionLotAsync(
        int canvasItemId,
        decimal minPrice,
        decimal startPrice,
        decimal minIncrement,
        DateTime endTime)
    {
        if (await _context.AuctionLots.AnyAsync(al =>
            al.CanvasItemId == canvasItemId &&
            al.Status == AuctionLotStatus.Active))
        {
            throw new InvalidOperationException("Этот холст уже участвует в активном аукционе");
        }

        if (endTime < DateTime.UtcNow.AddHours(24))
        {
            throw new InvalidOperationException("Аукцион должен длиться минимум 24 часа");
        }

        var lot = new AuctionLot
        {
            CanvasItemId = canvasItemId,
            EndTime = endTime,
            MinimumPrice = minPrice,
            StartingPrice = startPrice,
            MinBidIncrement = minIncrement
        };

        await _context.AuctionLots.AddAsync(lot);
        await _context.SaveChangesAsync();

        return lot;
    }

    public async Task<Bid> PlaceBidAsync(int lotId, string userId, decimal amount)
    {
        // Явно создаем транзакцию с уровнем изоляции
        using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

        try
        {
            var lot = await _context.AuctionLots
                .Include(al => al.Bids)
                .FirstOrDefaultAsync(al => al.Id == lotId);

            if (lot == null || lot.Status != AuctionLotStatus.Active)
                throw new InvalidOperationException("Lot is not available");

            var lastBid = lot.Bids.OrderByDescending(b => b.BidTime).FirstOrDefault();
            var minBid = lastBid?.Amount + lot.MinBidIncrement ?? lot.StartingPrice;

            if (amount < minBid)
                throw new InvalidOperationException("Bid amount is too low");

            var availableBalance = await _balanceService.GetAvailableBalanceAsync(userId);
            if (availableBalance + (lastBid == null ? 0 : lastBid.Amount) < amount)
                throw new InvalidOperationException("Insufficient funds");

            var bid = new Bid
            {
                LotId = lotId,
                UserId = userId,
                Amount = amount,
                BidTime = DateTime.UtcNow
            };

            // Добавление ставки
            await _context.Bids.AddAsync(bid);
            await _context.SaveChangesAsync();

            // Возврат предыдущей ставки
            if (lastBid != null)
            {
                await _balanceService.UnfreezeFundsAsync(lastBid.UserId, lastBid.Amount, lastBid.Id);
            }

            // Заморозка средств
            await _balanceService.FreezeFundsAsync(userId, amount, bid.Id);

            await transaction.CommitAsync();
            return bid;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task CancelBidAsync(int bidId, string userId)
    {
        var bid = await _context.Bids
            .Include(b => b.Lot)
            .FirstOrDefaultAsync(b => b.Id == bidId && b.UserId == userId);

        if (bid == null || bid.Lot.Status != AuctionLotStatus.Active)
            throw new InvalidOperationException("Cannot cancel bid");

        var isLastBid = await _context.Bids
            .Where(b => b.LotId == bid.LotId)
            .OrderByDescending(b => b.BidTime)
            .FirstOrDefaultAsync() == bid;

        if (!isLastBid)
            throw new InvalidOperationException("Cannot cancel non-last bid");

        await _balanceService.UnfreezeFundsAsync(userId, bid.Amount, bid.Id);
        _context.Bids.Remove(bid);
        await _context.SaveChangesAsync();
    }

    public async Task CompleteAuctionAsync(int lotId)
    {
        var lot = await _context.AuctionLots
            .Include(al => al.Bids)
            .Include(al => al.CanvasItem)
            .FirstOrDefaultAsync(al => al.Id == lotId);

        if (lot == null || lot.Status != AuctionLotStatus.Active)
            return;

        var winningBid = lot.Bids.OrderByDescending(b => b.Amount).FirstOrDefault();

        if (winningBid != null && winningBid.Amount >= lot.MinimumPrice)
        {
            // Перевод средств
            await _balanceService.TransferFundsAsync(
                winningBid.UserId,
                lot.CanvasItem.UserId,
                winningBid.Amount);
        }
        else
        {
            // Возврат всех ставок
            foreach (var bid in lot.Bids)
            {
                await _balanceService.UnfreezeFundsAsync(bid.UserId, bid.Amount, bid.Id);
            }
        }

        lot.Status = AuctionLotStatus.Completed;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AuctionLot>> GetUserActiveLotsAsync(string userId)
    {
        return await _context.AuctionLots
            .Include(al => al.CanvasItem)
            .Include(al => al.Bids)
            .Where(al => al.CanvasItem.UserId == userId &&
                        al.Status == AuctionLotStatus.Active &&
                        al.EndTime > DateTime.UtcNow)
            .OrderByDescending(al => al.EndTime)
            .ToListAsync();
    }

    public async Task CancelAuctionAsync(int lotId, string userId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        var lot = await _context.AuctionLots
            .Include(al => al.CanvasItem)
            .Include(al => al.Bids)
            .FirstOrDefaultAsync(al => al.Id == lotId);

        if (lot?.CanvasItem.UserId != userId || lot?.Status != AuctionLotStatus.Active)
            throw new InvalidOperationException("Cannot cancel auction");

        lot.Status = AuctionLotStatus.Cancelled;

        foreach (var bid in lot.Bids)
        {
            await _balanceService.UnfreezeFundsAsync(bid.UserId, bid.Amount, bid.Id);
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    public async Task<IEnumerable<AuctionLot>> GetActiveAuctionsAsync(int page, int pageSize)
    {
        return await _context.AuctionLots
            .Include(al => al.CanvasItem)
            .Include(al => al.Bids)
            .Where(al => al.Status == AuctionLotStatus.Active)
            .OrderBy(al => al.EndTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<AuctionLotDetailsDto> GetAuctionDetailsAsync(int lotId)
    {
        var lot = await _context.AuctionLots
            .Include(al => al.CanvasItem)
            .Include(al => al.Bids)
                .ThenInclude(b => b.User)
            .FirstOrDefaultAsync(al => al.Id == lotId);

        if (lot == null) return null;

        var currentPrice = lot.Bids.Any()
            ? lot.Bids.Max(b => b.Amount)
            : lot.StartingPrice;

        return new AuctionLotDetailsDto
        {
            Lot = lot,
            CurrentPrice = currentPrice,
            TimeLeft = lot.EndTime - DateTime.UtcNow
        };
    }
}
