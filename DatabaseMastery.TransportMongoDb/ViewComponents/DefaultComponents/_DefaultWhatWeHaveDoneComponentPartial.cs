using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.ViewComponents.DefaultComponents
{
    public class _DefaultWhatWeHaveDoneComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
