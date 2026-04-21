using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.ViewComponents.AdminComponents
{
    public class AdminLayoutScriptComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
