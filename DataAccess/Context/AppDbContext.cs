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
        }
    }
}
