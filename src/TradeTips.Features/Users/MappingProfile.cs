using AutoMapper;

namespace TradeTips.Features.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TradeTips.Domain.Person, UserDTO>(MemberList.None);
        }
    }
}
