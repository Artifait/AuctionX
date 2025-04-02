using System;

namespace AucX.Domain.Entities;

 public class AuctionLot
{
    public int Id { get; set; }
    public int GameItemId { get; set; }         
    public GameItem GameItem { get; set; } = null!;

    public DateTime EndTime { get; set; }       // Время завершения торгов
    public decimal CurrentBid { get; set; }     // Текущая ставка
    public decimal BuyOutPrice { get; set; }    // Сумма выкупа
    public decimal MinBidIncrement { get; set; } // Минимальное повышение ставки

    // Дополнительные свойства (например, история ставок) можно добавить по необходимости
}