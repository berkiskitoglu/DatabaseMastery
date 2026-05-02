using DatabaseMastery.TransportMongoDb.Services.QuestionServices;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.ViewComponents.DefaultComponents
{
    public class _DefaultFrequentlyAskedQuestionsComponentPartial : ViewComponent
    {
        private readonly IQuestionService _questionService;

        public _DefaultFrequentlyAskedQuestionsComponentPartial(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var faq_list = await _questionService.GetAllQuestionAsync();
            return View(faq_list);
        }
    }
}
