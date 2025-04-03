using System;

namespace AucX.Domain.Entities;

public class AppSettings
{
    public decimal CanvasUpgradePrice { get; set; }
    public decimal ColorPurchasePrice { get; set; }
    public int InitialCanvasWidth { get; set; }
    public int InitialCanvasHeight { get; set; }
    public int MaxCanvasWidth { get; set; }
    public int MaxCanvasHeight { get; set; }
    public List<ColorShopConfig> ColorShop { get; set; }
}

public class ColorShopConfig
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ColorCode { get; set; }
    public decimal Price { get; set; }
}