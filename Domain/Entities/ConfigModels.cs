
using System.Collections.Generic;

namespace AucX.Domain.Entities
{
    public class AppSettings
    {
        public decimal CanvasUpgradePrice { get; set; }
        public decimal ColorPurchasePrice { get; set; }
        public int InitialCanvasWidth { get; set; }
        public int InitialCanvasHeight { get; set; }
        public int MaxCanvasWidth { get; set; }
        public int MaxCanvasHeight { get; set; }
        // Добавляем свойство для магазина цветов
        public List<ColorShop> ColorShop { get; set; }
    }

    public class ColorShop
    {
        public List<ColorCollection> Collections { get; set; }
    }

    public class ColorCollection
    {
        // Для удобства можно задать набор коллекций: Default (бесплатная), Vibrant, Pastel, Neutrals
        public List<ColorItem> Default { get; set; }
        public List<ColorItem> Vibrant { get; set; }
        public List<ColorItem> Pastel { get; set; }
        public List<ColorItem> Neutrals { get; set; }
    }

    public class ColorItem
    {
        public string Name { get; set; }
        public string ColorCode { get; set; }
        public decimal Price { get; set; }
    }
}
