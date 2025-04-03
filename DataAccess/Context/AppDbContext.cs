using AucX.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AucX.DataAccess.Context
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<PixelArt> PixelArts { get; set; }
        public DbSet<PixelColor> PixelColors { get; set; }
        public DbSet<ColorShop> ColorShops { get; set; }
        public DbSet<UserColorPurchase> UserColorPurchases { get; set; }
        public DbSet<UserCanvasUpgrade> UserCanvasUpgrades { get; set; }
    }
}
