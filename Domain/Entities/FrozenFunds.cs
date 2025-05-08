using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AucX.Domain.Entities;

public class FrozenFunds
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime FrozenAt { get; set; } = DateTime.UtcNow;

    public int? BidId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual AppUser User { get; set; } = null!;

    [ForeignKey(nameof(BidId))]
    public virtual Bid? Bid { get; set; }
}
