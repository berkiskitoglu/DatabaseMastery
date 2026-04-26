using DatabaseMastery.TransportMongoDb.Dtos.BrandDtos;
using DatabaseMastery.TransportMongoDb.Services.BrandServices;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService BrandService)
        {
            _brandService = BrandService;
        }

        public async Task<IActionResult> BrandList()
        {
            var values = await _brandService.GetAllBrandAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateBrand()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBrand(CreateBrandDto createBrandDto)
        {
            await _brandService.CreateBrandAsync(createBrandDto);
            return RedirectToAction("BrandList");
        }
        public async Task<IActionResult> DeleteBrand(string id)
        {
            await _brandService.DeleteBrandAsync(id);
            return RedirectToAction("BrandList");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateBrand(string id)
        {
            var values = await _brandService.GetBrandByIdAsync(id);
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBrand(UpdateBrandDto updateBrandDto)
        {
            await _brandService.UpdateBrandAsync(updateBrandDto);
            return RedirectToAction("BrandList");
        }

    }
}
