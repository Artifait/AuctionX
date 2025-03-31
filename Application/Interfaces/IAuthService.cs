using AucX.Application.DTOs;

namespace AucX.Application.Interfaces;

public interface IAuthService
{
    Task RegisterUserAsync(RegisterUserDto dto);
    // Дополнительно: метод для входа
}