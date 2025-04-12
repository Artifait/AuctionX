    namespace AucX.Domain.Entities
    {
        public class CanvasItem
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public string PixelData { get; set; } = null!; // hex1,hex2,hex3...
            public int Width { get; set; }
            public int Height { get; set; }
            public string UserId { get; set; } = null!;
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        }
    }
