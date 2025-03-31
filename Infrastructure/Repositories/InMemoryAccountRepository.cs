using System.Collections.Concurrent;
using AucX.Domain.Entities;
using AucX.Infrastructure.Repositories;

namespace AucX.Infrastructure;

public class InMemoryAccountRepository : IAccountRepository
    {
        private readonly ConcurrentDictionary<string, Account> _accounts = new ConcurrentDictionary<string, Account>();

        public Task AddAsync(Account account)
        {
            _accounts[account.Username] = account;
            return Task.CompletedTask;
        }

        public Task<Account> GetByUsernameAsync(string username)
        {
            _accounts.TryGetValue(username, out var account);
            return Task.FromResult(account);
        }
    }