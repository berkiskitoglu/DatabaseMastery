using DatabaseMastery.TransportMongoDb.Dtos.GetInTouchDtos;
using DatabaseMastery.TransportMongoDb.Services.GetInTouchServices;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.Controllers
{
    public class GetInTouchController : Controller
    {
        private readonly IGetInTouchService _getInTouchService;

        public GetInTouchController(IGetInTouchService GetInTouchService)
        {
            _getInTouchService = GetInTouchService;
        }

        public async Task<IActionResult> GetInTouchList()
        {
            var values = await _getInTouchService.GetAllGetInTouchAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateGetInTouch()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateGetInTouch(CreateGetInTouchDto createGetInTouchDto)
        {
            await _getInTouchService.CreateGetInTouchAsync(createGetInTouchDto);
            return RedirectToAction("GetInTouchList");
        }
        public async Task<IActionResult> DeleteGetInTouch(string id)
        {
            await _getInTouchService.DeleteGetInTouchAsync(id);
            return RedirectToAction("GetInTouchList");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateGetInTouch(string id)
        {
            var values = await _getInTouchService.GetInTouchByIdAsync(id);
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateGetInTouch(UpdateGetInTouchDto updateGetInTouchDto)
        {
            await _getInTouchService.UpdateGetInTouchAsync(updateGetInTouchDto);
            return RedirectToAction("GetInTouchList");
        }

    }
}
