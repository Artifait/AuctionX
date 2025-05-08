using System;
using AucX.Domain.Entities;

namespace AucX.WebUI.DTO;

public class AuctionLotDetailsDto
{
    public AuctionLot Lot { get; set; }
    public decimal CurrentPrice { get; set; }
    public TimeSpan TimeLeft { get; set; }
}
