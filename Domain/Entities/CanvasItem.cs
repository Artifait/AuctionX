namespace AucX.Domain.Entities
{
    public class CanvasItem
    {
        public int Id { get; set; }

        // Название холста
        public string Name { get; set; } = null!;

        // Цвета, использованные на холсте
        public string ColorCodes { get; set; } = null!; // Список цветов в формате "#FF0000,#00FF00"

        // Размер холста
        public int Width { get; set; }
        public int Height { get; set; }

        // Ссылка на пользователя
        public string UserId { get; set; } = null!;
        public virtual AppUser User { get; set; } = null!;

        // Дата создания холста
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
