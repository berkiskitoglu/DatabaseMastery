using DatabaseMastery.TransportMongoDb.Services.ProjectSectionServices;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.ViewComponents.DefaultComponents
{
    public class _DefaultWhatWeHaveDoneComponentPartial : ViewComponent
    {
        private readonly IProjectSectionService _projectSectionService;

        public _DefaultWhatWeHaveDoneComponentPartial(IProjectSectionService projectSectionService)
        {
            _projectSectionService = projectSectionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var projectSection = await _projectSectionService.GetAllProjectSectionAsync();
            return View(projectSection);
        }
    }
}
