using AutoMapper;
using ResourceMetadata.Model;
using ResourceMetadata.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceMetadata.API.Mappers
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return "DomainToViewModelMappingProfile";
            }
        }


        protected override void Configure()
        {
            Mapper.CreateMap<ApplicationUser, UserProfileViewModel>();
            Mapper.CreateMap<ApplicationUser, UserViewModel>();
            Mapper.CreateMap<UserProfile, UserProfileViewModel>();
            Mapper.CreateMap<Resource, ResourceViewModel>();
            Mapper.CreateMap<Location, LocationViewModel>();
            Mapper.CreateMap<Setting, SettingViewModel>();
            Mapper.CreateMap<Category, CategoryViewModel>();
            Mapper.CreateMap<ResourceActivity,ResourceActivityViewModel>()
                .ForMember(vm => vm.ActivityDateString, dm=> dm.MapFrom(dModel => dModel.ActivityDate.ToLongDateString()));
            Mapper.CreateMap<ApplicationUser, RegisterViewModel>();
        }
    }
}