using AucX.DataAccess.Context;
using AucX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AucX.DataAccess.Repositories;

public interface IUserColorRepository
{
    Task<IEnumerable<UserColor>> GetPurchasedColorsAsync(string userId);
    Task<UserColor?> GetUserColorAsync(int id);
    Task AddUserColorAsync(UserColor userColor);
    Task RemoveUserColorAsync(UserColor userColor);
    Task SaveAsync();
}

public class UserColorRepository : IUserColorRepository
{
    private readonly AppDbContext _context;

    public UserColorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserColor>> GetPurchasedColorsAsync(string userId)
    {
        return await _context.UserColors
            .AsNoTracking()
            .Where(uc => uc.UserId == userId)
            .ToListAsync();
    }

    public async Task<UserColor?> GetUserColorAsync(int id)
    {
        return await _context.UserColors.FindAsync(id);
    }

    public async Task AddUserColorAsync(UserColor userColor)
    {
        await _context.UserColors.AddAsync(userColor);
    }

    public async Task RemoveUserColorAsync(UserColor userColor)
    {
        _context.UserColors.Remove(userColor);
        await Task.CompletedTask;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}