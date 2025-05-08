using System;
using AucX.WebUI.DTO;

namespace AucX.WebUI.Models;

public class AuctionLotViewModel
{
    public int Id { get; set; }
    public CanvasItemDto CanvasItem { get; set; }
    public decimal CurrentBid { get; set; }
    public decimal MinimumPrice { get; set; }
    public decimal StartingPrice { get; set; }
    public decimal MinBidIncrement { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan TimeLeft { get; set; }
    public List<BidDto> Bids { get; set; }
    public bool IsOwner { get; set; }
    public decimal AvailableBalance { get; set; }
    public decimal FrozenBalance { get; set; }
}
