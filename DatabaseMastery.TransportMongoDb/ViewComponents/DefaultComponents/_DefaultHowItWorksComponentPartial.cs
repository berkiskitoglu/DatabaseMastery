using DatabaseMastery.TransportMongoDb.Services.HowItWorkServices;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.ViewComponents.DefaultComponents
{
    public class _DefaultHowItWorksComponentPartial : ViewComponent
    {
        private readonly IHowItWorkService _howItWorkService;

        public _DefaultHowItWorksComponentPartial(IHowItWorkService howItWorkService)
        {
            _howItWorkService = howItWorkService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var howItWork = await _howItWorkService.GetAllHowItWorkAsync();
            return View(howItWork);
        }
    }
}
