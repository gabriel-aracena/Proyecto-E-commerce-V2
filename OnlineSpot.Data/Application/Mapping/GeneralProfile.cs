using AutoMapper;

using OnlineSpot.Data.Application.ViewModels;
using OnlineSpot.Data.Domain.Entities;



namespace OnlineSpot.Data.Application.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<User, UserViewModel>()
                .ReverseMap();

            CreateMap<User, SaveUserViewModel>()
                .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore());

            CreateMap<SaveUserViewModel, User>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => false));
                

        }

    }
}
