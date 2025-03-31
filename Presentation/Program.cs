
using AucX.Application;
using AucX.Application.Services;
using AucX.Infrastructure;
using AucX.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов
builder.Services.AddScoped<IAccountService, AccountService>(); // Реализация AccountService должна быть создана в Application
builder.Services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();

// Добавление аутентификации с использованием cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/account/login";
        options.LogoutPath = "/api/account/logout";
    });

builder.Services.AddControllers();

var app = builder.Build();

// Добавление мидлварей аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();