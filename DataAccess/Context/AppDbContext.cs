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
        // Добавляем новые DbSet
        public DbSet<AuctionLot> AuctionLots { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<FrozenFunds> FrozenFunds { get; set; }
        public DbSet<BannedUser> BannedUsers { get; set; }

        // Таблица для хранения купленных цветов
        public DbSet<UserColor> UserColors { get; set; }

        // Таблица для хранения холстов, разукрашенных пользователями
        public DbSet<CanvasItem> CanvasItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Конфигурация связи между AppUser и UserColor
            builder.Entity<UserColor>()
                   .HasOne(uc => uc.User)
                   .WithMany(u => u.PurchasedColors)
                   .HasForeignKey(uc => uc.UserId);

            // Дополнительные настройки для CanvasItem можно добавить при необходимости.
            // Например, установка уникального индекса по UserId, Width, Height и ColorCodes:
            builder.Entity<CanvasItem>()
                   .HasIndex(c => new { c.UserId, c.Width, c.Height, c.PixelData })
                   .IsUnique();

            builder.Entity<AuctionLot>()
                    .HasOne(al => al.CanvasItem)
                    .WithMany()
                    .HasForeignKey(al => al.CanvasItemId)
                    .OnDelete(DeleteBehavior.Restrict);

            // Конфигурация Bid
            builder.Entity<Bid>()
                    .HasIndex(b => new { b.LotId, b.BidTime })
                    .IsDescending(false, true);
                    
            // Конфигурация FrozenFunds
            builder.Entity<FrozenFunds>()
                .HasIndex(ff => ff.UserId);
        }
    }
}
