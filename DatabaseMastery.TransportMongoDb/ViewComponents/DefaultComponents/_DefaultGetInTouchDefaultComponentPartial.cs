using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.ViewComponents.DefaultComponents
{
    public class _DefaultGetInTouchDefaultComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
