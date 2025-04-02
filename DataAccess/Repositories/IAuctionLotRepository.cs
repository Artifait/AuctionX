using System;
using AucX.Domain.Entities;

namespace AucX.DataAccess.Repositories;

public interface IAuctionLotRepository
{
    Task<IEnumerable<AuctionLot>> GetAllAuctionLotsAsync();
    Task<AuctionLot> GetAuctionLotByIdAsync(int id);
    Task AddBidAsync(int lotId, decimal bidAmount);
}