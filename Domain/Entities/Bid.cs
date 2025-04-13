using System;

namespace AucX.Domain.Entities;

public class Bid
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public int LotId { get; set; }
    public decimal Amount { get; set; }
    public DateTime BidTime { get; set; } = DateTime.UtcNow;
    
    public virtual AppUser User { get; set; } = null!;
    public virtual AuctionLot Lot { get; set; } = null!;
}