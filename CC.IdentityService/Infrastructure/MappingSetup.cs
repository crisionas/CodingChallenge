using AutoMapper;
using CC.IdentityService.Models.Requests;
using CC.IdentityService.Repository.Entities;

namespace CC.IdentityService.Infrastructure
{
    public class MappingSetup : Profile
    {
        public MappingSetup()
        {
            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => Guid.NewGuid()));
        }
    }
}
