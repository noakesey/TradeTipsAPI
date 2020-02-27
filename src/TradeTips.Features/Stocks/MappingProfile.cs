using AutoMapper;

namespace TradeTips.Features.Stocks
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TradeTips.Domain.Stock, StockDetailDTO>(MemberList.None);
            CreateMap<StockSummaryDTO, TradeTips.Domain.Stock>(MemberList.None);
        }
    }
}
