using AutoMapper;
using DatabaseMastery.TransportMongoDb.Models;
using DatabaseMastery.TransportMongoDb.Services.ShipmentServices;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.Controllers
{
    public class TrackingController : Controller
    {
        private readonly IShipmentService _shipmentService;
        private readonly IMapper _mapper;

        public TrackingController(IShipmentService shipmentService, IMapper mapper)
        {
            _shipmentService = shipmentService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string? trackingNumber)
        {
            if (string.IsNullOrWhiteSpace(trackingNumber))
                return View(null as TrackingResultViewModel);

            var shipment = await _shipmentService.GetShipmentByTrackingNumberAsync(
                trackingNumber.Trim().ToUpper());

            if (shipment is null)
            {
                ViewBag.NotFound = true;
                ViewBag.SearchedNumber = trackingNumber;
                return View(null as TrackingResultViewModel);
            }

            var vm = _mapper.Map<TrackingResultViewModel>(shipment);
            return View(vm);
        }
    }
}