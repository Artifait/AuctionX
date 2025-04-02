using AucX.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AucX.WebUI.Controllers
{
public class AuctionController : Controller
    {
        private readonly IAuctionLotRepository _auctionLotRepository;

        public AuctionController(IAuctionLotRepository auctionLotRepository)
        {
            _auctionLotRepository = auctionLotRepository;
        }

        // GET: /Auction/Index
        public async Task<IActionResult> Index()
        {
            var lots = await _auctionLotRepository.GetAllAuctionLotsAsync();
            return View(lots);
        }

        // GET: /Auction/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var lot = await _auctionLotRepository.GetAuctionLotByIdAsync(id);
            if (lot == null)
                return NotFound();

            return View(lot);
        }

        // POST: /Auction/AddBid/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBid(int id, decimal bidAmount)
        {
            await _auctionLotRepository.AddBidAsync(id, bidAmount);
            return RedirectToAction("Details", new { id });
        }
    }
}
