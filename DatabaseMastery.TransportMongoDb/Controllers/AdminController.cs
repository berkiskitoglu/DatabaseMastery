using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
