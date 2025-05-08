using System;

namespace AucX.WebUI.Models;

public class AuctionIndexViewModel
{
    public List<AuctionLotDto> Lots { get; set; } = new();
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalCount { get; set; }
    public decimal UserBalance { get; set; }
    public decimal FrozenBalance { get; set; }
}

