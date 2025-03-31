using System.Security.Cryptography;
using System.Text;
using AucX.Application.DTOs;
using AucX.Application.Interfaces;
using AucX.Domain.Entities;
using AucX.Domain.Interfaces;

namespace AucX.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    
    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task RegisterUserAsync(RegisterUserDto dto)
    {
        // Простейшая валидация, можно добавить больше проверок
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            throw new ArgumentException("Email и пароль обязательны");
        
        // Хэширование пароля (простой пример, в продакшене используйте более безопасные алгоритмы)
        var passwordHash = ComputeSha256Hash(dto.Password);
        
        var user = new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            PasswordHash = passwordHash,
            RegisteredAt = DateTime.UtcNow
        };
        
        await _userRepository.AddAsync(user);
    }
    
    private string ComputeSha256Hash(string rawData)
    {
        using (var sha256Hash = SHA256.Create())
        {
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}