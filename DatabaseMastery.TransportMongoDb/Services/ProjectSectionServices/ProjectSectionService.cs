using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.ProjectSectionDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.ProjectSectionServices;

public class ProjectSectionService : IProjectSectionService
{
    private readonly IGenericRepository<ProjectSection> _repository;
    private readonly IMapper _mapper;

    public ProjectSectionService(IGenericRepository<ProjectSection> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task CreateProjectSectionAsync(CreateProjectSectionDto dto)
    {
        var entity = _mapper.Map<ProjectSection>(dto);
        await _repository.CreateAsync(entity);
    }

    public async Task DeleteProjectSectionAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<List<ResultProjectSectionDto>> GetAllProjectSectionAsync()
    {
        var values = await _repository.GetAllAsync();
        return _mapper.Map<List<ResultProjectSectionDto>>(values);
    }

    public async Task<GetProjectSectionByIdDto> GetProjectSectionByIdAsync(string id)
    {
        var value = await _repository.GetByIdAsync(id);
        return _mapper.Map<GetProjectSectionByIdDto>(value);
    }

    public async Task UpdateProjectSectionAsync(UpdateProjectSectionDto dto)
    {
        var entity = _mapper.Map<ProjectSection>(dto);
        await _repository.UpdateAsync(entity);
    }
}