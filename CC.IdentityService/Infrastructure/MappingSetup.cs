using AutoMapper;
using CC.IdentityService.Models;
using CC.IdentityService.Models.Requests;

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
