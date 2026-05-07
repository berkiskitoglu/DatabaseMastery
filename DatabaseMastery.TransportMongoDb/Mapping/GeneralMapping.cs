using AutoMapper;
using DatabaseMastery.TransportMongoDb.Dtos.AboutDtos;
using DatabaseMastery.TransportMongoDb.Dtos.BrandDtos;
using DatabaseMastery.TransportMongoDb.Dtos.GetInTouchDtos;
using DatabaseMastery.TransportMongoDb.Dtos.HowItWorkDtos;
using DatabaseMastery.TransportMongoDb.Dtos.OfferDtos;
using DatabaseMastery.TransportMongoDb.Dtos.ProjectSectionDtos;
using DatabaseMastery.TransportMongoDb.Dtos.QuestionDtos;
using DatabaseMastery.TransportMongoDb.Dtos.ShipmentDtos;
using DatabaseMastery.TransportMongoDb.Dtos.ShipmentTrackingDtos;
using DatabaseMastery.TransportMongoDb.Dtos.SliderDtos;
using DatabaseMastery.TransportMongoDb.Dtos.TestimonialDtos;
using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Models;

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

            // Offer Mapping

            CreateMap<CreateOfferDto, Offer>();
            CreateMap<UpdateOfferDto, Offer>();
            CreateMap<Offer, ResultOfferDto>();
            CreateMap<Offer, GetOfferByIdDto>();

            // About Mapping

            CreateMap<CreateAboutDto, About>();
            CreateMap<UpdateAboutDto, About>();
            CreateMap<About, ResultAboutDto>();
            CreateMap<About, GetAboutByIdDto>();

            // GetInTouch Mapping

            CreateMap<CreateGetInTouchDto, GetInTouch>();
            CreateMap<UpdateGetInTouchDto, GetInTouch>();
            CreateMap<GetInTouch, ResultGetInTouchDto>();
            CreateMap<GetInTouch, GetInTouchByIdDto>();

            // HowItWork Mapping

            CreateMap<CreateHowItWorkDto, HowItWork>();
            CreateMap<UpdateHowItWorkDto, HowItWork>();
            CreateMap<HowItWork, ResultHowItWorkDto>();
            CreateMap<HowItWork, GetHowItWorkByIdDto>();

            // Testimonial Mapping

            CreateMap<CreateTestimonialDto, Testimonial>();
            CreateMap<UpdateTestimonialDto, Testimonial>();
            CreateMap<Testimonial, ResultTestimonialDto>();
            CreateMap<Testimonial, GetTestimonialByIdDto>();

            // ProjectSection Mapping

            CreateMap<CreateProjectSectionDto, ProjectSection>();
            CreateMap<UpdateProjectSectionDto, ProjectSection>();
            CreateMap<ProjectSection, ResultProjectSectionDto>();
            CreateMap<ProjectSection, GetProjectSectionByIdDto>();

            // Question Mapping

            CreateMap<CreateQuestionDto, Question>();
            CreateMap<UpdateQuestionDto, Question>();
            CreateMap<Question, ResultQuestionDto>();
            CreateMap<Question, GetQuestionByIdDto>();

            CreateMap<Shipment, ResultShipmentDto>().ReverseMap();
            CreateMap<Shipment, CreateShipmentDto>().ReverseMap();
            CreateMap<Shipment, UpdateShipmentDto>().ReverseMap();
            CreateMap<Shipment, GetShipmentByIdDto>().ReverseMap();

            CreateMap<ShipmentTracking, CreateShipmentTrackingDto>().ReverseMap();
            CreateMap<ShipmentTracking, ResultShipmentTrackingDto>().ReverseMap();
            CreateMap<ShipmentTracking, UpdateShipmentTrackingDto>().ReverseMap();

            CreateMap<ShipmentTracking, TrackingEventViewModel>();
            CreateMap<GetShipmentByIdDto, TrackingResultViewModel>()
                .ForMember(dest => dest.Events, opt => opt.MapFrom(src =>
                    (src.Trackings ?? new List<ShipmentTracking>())
                        .OrderBy(t => t.EventDate)
                        .ToList()));
        }
    }
}
