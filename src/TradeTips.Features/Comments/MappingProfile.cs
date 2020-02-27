using AutoMapper;
using TradeTips.Features.Comments;

namespace TradeTips.Features.Comments
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TradeTips.Domain.Comment, CommentDTO>(MemberList.None);
            CreateMap<CommentDTO, TradeTips.Domain.Comment>(MemberList.None);
        }
    }
}
