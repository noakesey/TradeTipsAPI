using AutoMapper;

namespace TradeTips.Features.Profiles
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<TradeTips.Domain.Person, ProfileDTO>(MemberList.None);
        }
    }
}
