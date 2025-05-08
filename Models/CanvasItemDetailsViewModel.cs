using System.ComponentModel.DataAnnotations;

namespace AucX.WebUI.Models
{
    public class CanvasItemDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PixelData { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsOwner { get; set; }
        public bool CanBeAuctioned { get; set; }
        public CreateAuctionViewModel? AuctionForm { get; set; }
    }

    public class CreateAuctionViewModel
    {
        public int CanvasItemId { get; set; }
        
        [Required]
        [Range(1, 1000000)]
        public decimal StartingPrice { get; set; }
        
        [Required]
        [Range(1, 1000000)]
        public decimal MinimumPrice { get; set; }
        
        [Required]
        [Range(1, 1000000)]
        public decimal MinBidIncrement { get; set; }
        
        [Required]
        [FutureDate(24)] // Минимум 24 часа для аукциона
        public DateTime EndTime { get; set; }
    }

    // Кастомный валидатор для даты
    public class FutureDateAttribute : ValidationAttribute
    {
        private readonly int _minHours;

        public FutureDateAttribute(int minHours)
        {
            _minHours = minHours;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value is DateTime date)
            {
                if (date < DateTime.UtcNow.AddHours(_minHours))
                {
                    return new ValidationResult($"Дата должна быть минимум на {_minHours} часов позже текущего времени");
                }
            }
            return ValidationResult.Success;
        }
    }
}
