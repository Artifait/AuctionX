namespace AucX.WebUI.Models;

public class ProfileViewModel
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<CanvasItemDto> UserCanvasItems { get; set; }
    public List<AuctionLotDto> AuctionLots { get; set; }
    public decimal AvailableBalance { get; set; }
    public decimal FrozenBalance { get; set; }
}

public class CanvasItemDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PixelData { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public class AuctionLotDto
{
    public int Id { get; set; }
    public CanvasItemDto CanvasItem { get; set; }
    public decimal CurrentBid { get; set; }
    public decimal MinimumPrice { get; set; }
    public DateTime EndTime { get; set; }
}

