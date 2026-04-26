using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.BrandDtos;
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

            // Brand Mapping

            CreateMap<CreateBrandDto, Brand>();
            CreateMap<UpdateBrandDto, Brand>();
            CreateMap<Brand, ResultBrandDto>();
            CreateMap<Brand, GetBrandByIdDto>();
        }
    }
}
