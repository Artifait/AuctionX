using System;
using System.Collections.Generic;
using AucX.Domain.Entities;

namespace AucX.Domain.Entities
{
    public class PixelArt
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }  // Ширина холста
        public int Height { get; set; } // Высота холста
        public List<PixelColor> Colors { get; set; } = new List<PixelColor>(); // Список использованных цветов
        public string OwnerId { get; set; }  // Связь с пользователем (для аукциона)
        public AppUser Owner { get; set; }
    }

    public class PixelColor
    {
        public int Id { get; set; }
        public string ColorCode { get; set; }  // HEX код цвета
    }
}
