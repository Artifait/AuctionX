namespace AucX.Domain.Entities
{
    public class UserCanvasUpgrade
    {
        public int Id { get; set; }
        public string UserId { get; set; }  // Связь с пользователем
        public AppUser User { get; set; }
        public int MaxWidth { get; set; } = 32;  // Максимальная ширина холста
        public int MaxHeight { get; set; } = 32; // Максимальная высота холста
        public decimal Price { get; set; } = 3000; // Цена улучшения
    }
}
