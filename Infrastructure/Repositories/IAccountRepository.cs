using AucX.Domain.Entities;

namespace AucX.Infrastructure.Repositories;

public interface IAccountRepository
{
    Task AddAsync(Account account);
    Task<Account> GetByUsernameAsync(string username);
}