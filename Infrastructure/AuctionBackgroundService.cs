
using AucX.DataAccess.Context;
using AucX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AucX.WebUI.Infrastructure;

public class AuctionBackgroundService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<AuctionBackgroundService> _logger;

    public AuctionBackgroundService(IServiceProvider services, ILogger<AuctionBackgroundService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _services.CreateScope();
            var auctionService = scope.ServiceProvider.GetRequiredService<IAuctionService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            var expiredLots = await dbContext.AuctionLots
                .Where(al => al.EndTime <= DateTime.UtcNow && al.Status == AuctionLotStatus.Active)
                .ToListAsync(stoppingToken);

            foreach (var lot in expiredLots)
            {
                try 
                {
                    await auctionService.CompleteAuctionAsync(lot.Id);
                    _logger.LogInformation("Completed auction lot {LotId}", lot.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error completing auction lot {LotId}", lot.Id);
                }
            }
            
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
