using Microsoft.AspNetCore.Identity;

namespace AucX.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public required string FullName { get; set; }
        
        // Новое поле для хранения баланса (например, количество кликов или валюты)
        public decimal Balance { get; set; } = 0; // Инициализируем нулевым значением
    }
}
