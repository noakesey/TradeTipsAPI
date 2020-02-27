using AutoMapper;

namespace TradeTips.Features.Articles
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<ArticleEditDTO, TradeTips.Domain.Article>();
            CreateMap<ArticleDetailDTO, TradeTips.Domain.Article>();

            CreateMap<TradeTips.Domain.Article, ArticleDetailDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Author.Username));

            CreateMap<TradeTips.Domain.Article, ArticleListDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Author.Username));
        }
    }
}
