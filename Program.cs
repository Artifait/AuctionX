using AucX.DataAccess.Context;
using AucX.DataAccess.Repositories;
using AucX.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Конфигурация подключения к базе данных (например, SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем Identity с кастомной моделью пользователя
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        // Настройка параметров (например, требования к паролю)
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//builder.Services.AddTransient<IAuctionLotRepository, AuctionLotRepository>();
builder.Services.AddTransient<IUserCanvasUpgradeService, UserCanvasUpgradeService>();
builder.Services.AddTransient<IPixelArtRepository, PixelArtRepository>();
builder.Services.AddTransient<IColorShopRepository, ColorShopRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Использование стандартных middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Добавляем аутентификацию и авторизацию
app.UseAuthentication();
app.UseAuthorization();

// Настройка маршрутов
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
