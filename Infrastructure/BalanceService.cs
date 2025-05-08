using System;
using AucX.DataAccess.Context;
using AucX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AucX.WebUI.Infrastructure;

public interface IBalanceService
{
    Task<decimal> GetAvailableBalanceAsync(string userId);
    Task<decimal> GetFrozenBalanceAsync(string userId);
    Task<decimal> GetBalanceAsync(string userId);
    Task FreezeFundsAsync(string userId, decimal amount, int bidId);
    Task UnfreezeFundsAsync(string userId, decimal amount, int bidId);
    Task TransferFundsAsync(string fromUserId, string toUserId, decimal amount);
}

public class BalanceService : IBalanceService
{
    private readonly AppDbContext _context;

    public BalanceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<decimal> GetAvailableBalanceAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        var frozen = await _context.FrozenFunds
            .Where(ff => ff.UserId == userId)
            .SumAsync(ff => ff.Amount);

        return user!.Balance - frozen;
    }

    public async Task<decimal> GetBalanceAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);

        return user!.Balance;
    }

    public async Task<decimal> GetFrozenBalanceAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        var frozen = await _context.FrozenFunds
            .Where(ff => ff.UserId == userId)
            .SumAsync(ff => ff.Amount);

        return frozen;
    }

    public async Task FreezeFundsAsync(string userId, decimal amount, int bidId)
    {
        var frozenFunds = new FrozenFunds
        {
            UserId = userId,
            Amount = amount,
            BidId = bidId
        };

        await _context.FrozenFunds.AddAsync(frozenFunds);
        await _context.SaveChangesAsync();
    }

    public async Task UnfreezeFundsAsync(string userId, decimal amount, int bidId)
    {
        var frozenFund = await _context.FrozenFunds
            .FirstOrDefaultAsync(ff => ff.UserId == userId && ff.BidId == bidId);

        if (frozenFund != null)
        {
            _context.FrozenFunds.Remove(frozenFund);
            await _context.SaveChangesAsync();
        }
    }

    public async Task TransferFundsAsync(string fromUserId, string toUserId, decimal amount)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var fromUser = await _context.Users.FindAsync(fromUserId);
            var toUser = await _context.Users.FindAsync(toUserId);

            if (fromUser!.Balance < amount)
                throw new InvalidOperationException("Insufficient funds");

            fromUser.Balance -= amount;
            toUser!.Balance += amount;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
