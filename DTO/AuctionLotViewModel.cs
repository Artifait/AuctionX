using System;
using AucX.WebUI.Models;

namespace AucX.WebUI.DTO;

public class BidDto
{
    public string UserName { get; set; }
    public decimal Amount { get; set; }
    public DateTime BidTime { get; set; }
}
