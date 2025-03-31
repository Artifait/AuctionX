using AucX.Application.DTOs;

namespace AucX.Application;

public interface IAccountService
{
    Task<AccountDto> RegisterAsync(RegisterAccountDto dto);
    Task<AccountDto> LoginAsync(string username, string password);
}