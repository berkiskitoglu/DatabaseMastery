using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.ViewComponents.DefaultComponents
{
    public class _DefaultOfferComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
