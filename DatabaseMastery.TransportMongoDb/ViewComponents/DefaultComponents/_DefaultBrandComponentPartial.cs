using DatabaseMastery.TransportMongoDb.Services.BrandServices;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.ViewComponents.DefaultComponents
{
    public class _DefaultBrandComponentPartial : ViewComponent
    {
        private readonly IBrandService _brandService;

        public _DefaultBrandComponentPartial(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var brandList = await _brandService.GetAllBrandAsync();
            return View(brandList);
        }
    }
}
