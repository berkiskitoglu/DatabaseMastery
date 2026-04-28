using DatabaseMastery.TransportMongoDb.Services.GetInTouchServices;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.ViewComponents.DefaultComponents
{
    public class _DefaultGetInTouchComponentPartial : ViewComponent
    {
        private readonly IGetInTouchService _getInTouchService;

        public _DefaultGetInTouchComponentPartial(IGetInTouchService getInTouchService)
        {
            _getInTouchService = getInTouchService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var getInTouchInfo = await _getInTouchService.GetAllGetInTouchAsync();
            return View(getInTouchInfo.FirstOrDefault());
        }
    }
}
