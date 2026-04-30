using DatabaseMastery.TransportMongoDb.Services.TestimonialServices;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.ViewComponents.DefaultComponents
{
    public class _DefaultTestimonialComponentPartial : ViewComponent
    {
        private readonly ITestimonialService _testimonialService;

        public _DefaultTestimonialComponentPartial(ITestimonialService testimonialService)
        {
            _testimonialService = testimonialService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var testimonialList = await _testimonialService.GetAllTestimonialAsync();
            return View(testimonialList);
        }
    }
}
