using DatabaseMastery.TransportMongoDb.Dtos.ProjectSectionDtos;

namespace DatabaseMastery.TransportMongoDb.Services.ProjectSectionServices
{
    public interface IProjectSectionService
    {
        Task<List<ResultProjectSectionDto>> GetAllProjectSectionAsync();
        Task CreateProjectSectionAsync(CreateProjectSectionDto createProjectSectionDto);
        Task UpdateProjectSectionAsync(UpdateProjectSectionDto updateProjectSectionDto);
        Task<GetProjectSectionByIdDto> GetProjectSectionByIdAsync(string id);
        Task DeleteProjectSectionAsync(string id);
    }
}
