using DatabaseMastery.TransportMongoDb.Dtos.ProjectSectionDtos;
using DatabaseMastery.TransportMongoDb.Services.ProjectSectionServices;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseMastery.TransportMongoDb.Controllers
{
    public class ProjectSectionController : Controller
    {
        private readonly IProjectSectionService _projectSectionService;

        public ProjectSectionController(IProjectSectionService ProjectSectionService)
        {
            _projectSectionService = ProjectSectionService;
        }

        public async Task<IActionResult> ProjectSectionList()
        {
            var values = await _projectSectionService.GetAllProjectSectionAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateProjectSection()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateProjectSection(CreateProjectSectionDto createProjectSectionDto)
        {
            await _projectSectionService.CreateProjectSectionAsync(createProjectSectionDto);
            return RedirectToAction("ProjectSectionList");
        }
        public async Task<IActionResult> DeleteProjectSection(string id)
        {
            await _projectSectionService.DeleteProjectSectionAsync(id);
            return RedirectToAction("ProjectSectionList");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateProjectSection(string id)
        {
            var values = await _projectSectionService.GetProjectSectionByIdAsync(id);
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProjectSection(UpdateProjectSectionDto updateProjectSectionDto)
        {
            await _projectSectionService.UpdateProjectSectionAsync(updateProjectSectionDto);
            return RedirectToAction("ProjectSectionList");
        }

    }
}
