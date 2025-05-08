using AucX.DataAccess.Context;
using AucX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AucX.DataAccess.Repositories;

public interface IBannedUserRepository
{
    Task BanUserAsync(string userId, string reason);
    Task UnbanUserAsync(string userId);
    Task<bool> IsUserBannedAsync(string userId);
    Task<BannedUser?> GetBannedUserAsync(string userId);
}

public class BannedUserRepository : IBannedUserRepository
{
    private readonly AppDbContext _context;

    public BannedUserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task BanUserAsync(string userId, string reason)
    {
        var bannedUser = new BannedUser { UserId = userId, Reason = reason };
        _context.BannedUsers.Add(bannedUser);
        await _context.SaveChangesAsync();
    }

    public async Task UnbanUserAsync(string userId)
    {
        var bannedUser = await _context.BannedUsers.FirstOrDefaultAsync(b => b.UserId == userId);
        if (bannedUser != null)
        {
            _context.BannedUsers.Remove(bannedUser);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsUserBannedAsync(string userId)
    {
        return await _context.BannedUsers.AnyAsync(b => b.UserId == userId);
    }

    public async Task<BannedUser?> GetBannedUserAsync(string userId)
    {
        return await _context.BannedUsers.FirstOrDefaultAsync(b => b.UserId == userId);
    }
}