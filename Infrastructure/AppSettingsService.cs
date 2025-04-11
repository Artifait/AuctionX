using System;

namespace AucX.WebUI.Infrastructure;

public interface IAppSettingsService
{
    List<ColorCollection> GetColorCollections();
    int GetCanvasUpgradePrice();
    int GetColorPurchasePrice();
    (int width, int height) GetInitialCanvasSize();
    (int maxWidth, int maxHeight) GetMaxCanvasSize();
}

public class AppSettingsService : IAppSettingsService
{
    private readonly IConfiguration _configuration;

    public AppSettingsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

public List<ColorCollection> GetColorCollections()
{
    var section = _configuration.GetSection("AppSettings:ColorShop");
    var colorShops = section.Get<List<ColorShopWrapper>>();
    // Если необходимо получить коллекции из первого объекта
    return colorShops?.FirstOrDefault()?.Collections ?? new List<ColorCollection>();
}


    public int GetCanvasUpgradePrice()
    {
        return _configuration.GetValue<int>("AppSettings:CanvasUpgradePrice");
    }

    public int GetColorPurchasePrice()
    {
        return _configuration.GetValue<int>("AppSettings:ColorPurchasePrice");
    }

    public (int width, int height) GetInitialCanvasSize()
    {
        var width = _configuration.GetValue<int>("AppSettings:InitialCanvasWidth");
        var height = _configuration.GetValue<int>("AppSettings:InitialCanvasHeight");
        return (width, height);
    }

    public (int maxWidth, int maxHeight) GetMaxCanvasSize()
    {
        var maxWidth = _configuration.GetValue<int>("AppSettings:MaxCanvasWidth");
        var maxHeight = _configuration.GetValue<int>("AppSettings:MaxCanvasHeight");
        return (maxWidth, maxHeight);
    }
}

public class ColorCollection
{
    public List<Color> Default { get; set; } = null!;
    public List<Color> Vibrant { get; set; } = null!;
    public List<Color> Pastel { get; set; } = null!;
    public List<Color> Neutrals { get; set; } = null!;
}

public class Color
{
    public string Name { get; set; } = null!;
    public string ColorCode { get; set; } = null!;
    public int Price { get; set; }
}

public class ColorShopWrapper
{
    public List<ColorCollection> Collections { get; set; } = new List<ColorCollection>();
}
