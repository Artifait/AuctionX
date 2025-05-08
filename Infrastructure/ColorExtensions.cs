namespace AucX.WebUI.Infrastructure
{
    public static class ColorExtensions
    {
        public static IEnumerable<Color> GetColors(this ColorCollection collection)
        {
            return collection.Default.Select(c => new Color { Category = "Default", Name = c.Name, ColorCode = c.ColorCode, Price = c.Price })
                .Concat(collection.Vibrant.Select(c => new Color { Category = "Vibrant", Name = c.Name, ColorCode = c.ColorCode, Price = c.Price }))
                .Concat(collection.Pastel.Select(c => new Color { Category = "Pastel", Name = c.Name, ColorCode = c.ColorCode, Price = c.Price }))
                .Concat(collection.Neutrals.Select(c => new Color { Category = "Neutrals", Name = c.Name, ColorCode = c.ColorCode, Price = c.Price }));
        }
    }
}