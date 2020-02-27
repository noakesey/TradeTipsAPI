using AutoMapper;

namespace TradeTips.Features.Prices
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TradeTips.Domain.DailyPrice, PriceDTO>(MemberList.None);
            CreateMap<PriceDTO, TradeTips.Domain.DailyPrice>(MemberList.None);

            CreateMap<TradeTips.Domain.IntraDayPrice, PriceDTO>(MemberList.None);
            CreateMap<PriceDTO, TradeTips.Domain.IntraDayPrice>(MemberList.None);
        }
    }
}
