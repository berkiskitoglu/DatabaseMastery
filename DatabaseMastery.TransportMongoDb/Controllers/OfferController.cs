using DatabaseMastery.TransportMongoDb.Dtos.OfferDtos;
using DatabaseMastery.TransportMongoDb.Services.OfferServices;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.Controllers
{
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService OfferService)
        {
            _offerService = OfferService;
        }

        public async Task<IActionResult> OfferList()
        {
            var values = await _offerService.GetAllOfferAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateOffer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateOffer(CreateOfferDto createOfferDto)
        {
            await _offerService.CreateOfferAsync(createOfferDto);
            return RedirectToAction("OfferList");
        }
        public async Task<IActionResult> DeleteOffer(string id)
        {
            await _offerService.DeleteOfferAsync(id);
            return RedirectToAction("OfferList");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateOffer(string id)
        {
            var values = await _offerService.GetOfferByIdAsync(id);
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOffer(UpdateOfferDto updateOfferDto)
        {
            await _offerService.UpdateOfferAsync(updateOfferDto);
            return RedirectToAction("OfferList");
        }

    }
}
