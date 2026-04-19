using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.ViewComponents.DefaultComponents
{
    public class _DefaultSliderComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
