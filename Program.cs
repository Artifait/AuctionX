using AucX.DataAccess.Context;
using AucX.DataAccess.Repositories;
using AucX.Domain.Entities;
using AucX.WebUI.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAppSettingsService, AppSettingsService>();

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
builder.Services.AddScoped<ICanvasItemRepository, CanvasItemRepository>();
builder.Services.AddScoped<IUserColorRepository, UserColorRepository>();


builder.Services.AddControllersWithViews()    
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });;

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
