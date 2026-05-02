using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.QuestionDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.QuestionServices;

public class QuestionService : IQuestionService
{
    private readonly IGenericRepository<Question> _repository;
    private readonly IMapper _mapper;

    public QuestionService(IGenericRepository<Question> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task CreateQuestionAsync(CreateQuestionDto dto)
    {
        var entity = _mapper.Map<Question>(dto);
        await _repository.CreateAsync(entity);
    }

    public async Task DeleteQuestionAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<List<ResultQuestionDto>> GetAllQuestionAsync()
    {
        var values = await _repository.GetAllAsync();
        return _mapper.Map<List<ResultQuestionDto>>(values);
    }

    public async Task<GetQuestionByIdDto> GetQuestionByIdAsync(string id)
    {
        var value = await _repository.GetByIdAsync(id);
        return _mapper.Map<GetQuestionByIdDto>(value);
    }

    public async Task UpdateQuestionAsync(UpdateQuestionDto dto)
    {
        var entity = _mapper.Map<Question>(dto);
        await _repository.UpdateAsync(entity);
    }
}