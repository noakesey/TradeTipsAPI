using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;
using TradeTips.Security;

namespace TradeTips.Features.Stocks
{
    public class ListHandler : IRequestHandler<ListQuery, StocksEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMapper _mapper;

        public ListHandler(TradeTipsContext context, ICurrentUserAccessor currentUserAccessor, IMapper mapper)
        {
            _context = context;
            _currentUserAccessor = currentUserAccessor;
            _mapper = mapper;
        }

        public async Task<StocksEnvelopeDTO> Handle(ListQuery message, CancellationToken cancellationToken)
        {
            //TODO Investigate ProjectTo
            //var stocks = _context.Stocks
            //    .Where(s => s.Obsolete == false)
            //    .ProjectTo<StockDTO>().ToList();

            var stocks = from s in _context.Stocks
                         where s.Obsolete == false
                select new StockDetailDTO
                {
                    StockId = s.StockId,
                    Name = s.Name,
                    BargainShare = s.BargainShare,
                    Obsolete = s.Obsolete,
                    FirstPublicationDate = s.Articles.Min(a => a.PublicationDate),
                    LatestPublicationDate = s.Articles.Max(a => a.PublicationDate),
                    LatestDailyPriceDate = s.DailyPrices.Max(p => p.TransDate)
                };

            var dtos = await stocks
                .Skip(message.Offset ?? 0)
                .Take(message.Limit ?? 500)
                .AsNoTracking()
                .OrderBy(s => s.LatestPublicationDate)
                .ToListAsync();

            return new StocksEnvelopeDTO
            {
                Stocks = dtos,
                StocksCount = stocks.Count()
            };

        }
    }
}