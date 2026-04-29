using DatabaseMastery.TransportMongoDb.Dtos.HowItWorkDtos;
using DatabaseMastery.TransportMongoDb.Services.HowItWorkServices;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.Controllers
{
    public class HowItWorkController : Controller
    {
        private readonly IHowItWorkService _HowItWorkService;

        public HowItWorkController(IHowItWorkService HowItWorkService)
        {
            _HowItWorkService = HowItWorkService;
        }

        public async Task<IActionResult> HowItWorkList()
        {
            var values = await _HowItWorkService.GetAllHowItWorkAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateHowItWork()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateHowItWork(CreateHowItWorkDto createHowItWorkDto)
        {
            await _HowItWorkService.CreateHowItWorkAsync(createHowItWorkDto);
            return RedirectToAction("HowItWorkList");
        }
        public async Task<IActionResult> DeleteHowItWork(string id)
        {
            await _HowItWorkService.DeleteHowItWorkAsync(id);
            return RedirectToAction("HowItWorkList");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateHowItWork(string id)
        {
            var values = await _HowItWorkService.GetHowItWorkByIdAsync(id);
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateHowItWork(UpdateHowItWorkDto updateHowItWorkDto)
        {
            await _HowItWorkService.UpdateHowItWorkAsync(updateHowItWorkDto);
            return RedirectToAction("HowItWorkList");
        }

    }
}
