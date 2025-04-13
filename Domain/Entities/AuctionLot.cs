using System;

namespace AucX.Domain.Entities;

public class AuctionLot
{
    public int Id { get; set; }
    public int CanvasItemId { get; set; }
    public DateTime EndTime { get; set; }
    public decimal MinimumPrice { get; set; }
    public decimal StartingPrice { get; set; }
    public decimal MinBidIncrement { get; set; }
    public AuctionLotStatus Status { get; set; } = AuctionLotStatus.Active;
    
    public virtual CanvasItem CanvasItem { get; set; } = null!;
    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
}

public enum AuctionLotStatus
{
    Active,
    Completed,
    Cancelled
}