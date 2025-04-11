using Microsoft.AspNetCore.Identity;

namespace AucX.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public required string FullName { get; set; }
        
        // Баланс пользователя
        public decimal Balance { get; set; } = 0;

        // Текущий максимальный размер холста
        public int MaxCanvasWidth { get; set; } = 2;
        public int MaxCanvasHeight { get; set; } = 2;

        // Список купленных пользователем цветов
        public virtual ICollection<UserColor> PurchasedColors { get; set; } = new List<UserColor>();
        
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}
