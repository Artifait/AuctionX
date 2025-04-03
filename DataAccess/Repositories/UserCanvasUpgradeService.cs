using AucX.DataAccess.Context;
using AucX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace AucX.DataAccess.Repositories
{
    public class UserCanvasUpgradeService : IUserCanvasUpgradeService
    {
        private readonly AppDbContext _context;
        private readonly AppSettings _appSettings;

        public UserCanvasUpgradeService(AppDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<bool> UpgradeCanvasAsync(string userId)
        {
            var userUpgrade = await _context.UserCanvasUpgrades
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (userUpgrade == null)
            {
                userUpgrade = new UserCanvasUpgrade
                {
                    UserId = userId,
                    MaxWidth = _appSettings.InitialCanvasWidth,
                    MaxHeight = _appSettings.InitialCanvasHeight
                };
                _context.UserCanvasUpgrades.Add(userUpgrade);
            }
            else
            {
                userUpgrade.MaxWidth = Math.Min(userUpgrade.MaxWidth + 1, _appSettings.MaxCanvasWidth);
                userUpgrade.MaxHeight = Math.Min(userUpgrade.MaxHeight + 1, _appSettings.MaxCanvasHeight);
            }

            // Списываем средства с пользователя
            var user = await _context.Users.FindAsync(userId);
            //user.Balance -= _appSettings.CanvasUpgradePrice;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
