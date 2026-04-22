using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.SliderDtos;
using DatabaseMastery.TransportMongoDb.Entities;

namespace DatabaseMastery.TransportMongoDb.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {

            // Slider Mappings

            CreateMap<CreateSliderDto,Slider>();
            CreateMap<UpdateSliderDto,Slider>();
            CreateMap<Slider,ResultSliderDto>();
            CreateMap<Slider,GetSliderByIdDto>();
        }
    }
}
