using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AucX.Domain.Entities
{
    public class Bid
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(Lot))]
        public int LotId { get; set; }

        public decimal Amount { get; set; }
        public DateTime BidTime { get; set; } = DateTime.UtcNow;

        public virtual AppUser User { get; set; } = null!;
        public virtual AuctionLot Lot { get; set; } = null!;
    }
}
