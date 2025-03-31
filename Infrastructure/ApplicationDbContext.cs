using AucX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AucX.Infrastructure;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Конфигурация модели (ограничения, индексы и т.д.)
        }
    }