using System.Security.Cryptography;
using System.Text;
using AucX.Application.DTOs;
using AucX.Domain.Entities;

namespace AucX.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountDto> RegisterAsync(RegisterAccountDto dto)
    {
        // Простое хеширование пароля (для продакшена использовать более надежные алгоритмы)
        var passwordHash = ComputeSha256Hash(dto.Password);
        var account = new Account(dto.Username, dto.Email, passwordHash);
        await _accountRepository.AddAsync(account);
        return new AccountDto { Id = account.Id, Username = account.Username, Email = account.Email };
    }

    public async Task<AccountDto> LoginAsync(string username, string password)
    {
        var account = await _accountRepository.GetByUsernameAsync(username);
        if (account == null)
            return null;

        var passwordHash = ComputeSha256Hash(password);
        if (account.PasswordHash != passwordHash)
            return null;

        return new AccountDto { Id = account.Id, Username = account.Username, Email = account.Email };
    }

    private string ComputeSha256Hash(string rawData)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}