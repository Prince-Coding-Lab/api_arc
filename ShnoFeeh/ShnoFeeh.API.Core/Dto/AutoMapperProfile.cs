using AutoMapper;
using ShnoFeeh.API.Core.Entities;

namespace ShnoFeeh.API.Core.Dto
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddContactUsDto, ContactUs>();
            CreateMap<UpdateContactUsDto, ContactUs>();
            CreateMap<UserCreateDto, AdminUsers>();
            CreateMap<UserUpdateDto, AdminUsers>();
            CreateMap<AddCategoryDto, Categories>();
            CreateMap<UpdateCategoryDto, Categories>()
                 .ForMember(cat => cat.Id, opt => opt.MapFrom(src => src.CategoryId));
            CreateMap<UpdateCountryDto, Countries>()
                .ForMember(cat => cat.Id, opt => opt.MapFrom(src => src.CountryId));
            CreateMap<AddAdsDto, Ads>();
            CreateMap<UpdateAdsDto, Ads>()
                          .ForMember(cat => cat.Id, opt => opt.MapFrom(src => src.AdId));
            CreateMap<AddCampaignDto, Campaign>();
            CreateMap<UpdateCampaignDto, Campaign>()
                   .ForMember(cat => cat.Id, opt => opt.MapFrom(src => src.CampaignId));
            CreateMap<AddPaymentReferenceDto, PaymentReference>();
            CreateMap<UpdateAdsPricesDto, AdsPrices>()
                     .ForMember(cat => cat.Id, opt => opt.MapFrom(src => src.AdPriceId));
            CreateMap<AddOrderDto, Orders>();
            CreateMap<UpdateOrderDto, Orders>()
                .ForMember(or => or.Id, opt => opt.MapFrom(src => src.OrderId));
            CreateMap<AddCityDto, City>();
            CreateMap<UpdateCityDto, City>();
            CreateMap<ExceptionLogCreateDto, ExceptionWeb>();
            CreateMap<AddAdvertismentDto, Advertisments>();
            CreateMap<UpdateAdvertismentDto, Advertisments>()
              .ForMember(or => or.Id, opt => opt.MapFrom(src => src.AdvertismentId));
        }
    }
}
