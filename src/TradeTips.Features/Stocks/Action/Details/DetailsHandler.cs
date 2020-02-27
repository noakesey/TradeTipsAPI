using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;
using System.Linq;

namespace TradeTips.Features.Stocks
{
    public class DetailsHandler : IRequestHandler<DetailsQuery, StockEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly IMapper _mapper;

        public DetailsHandler(TradeTipsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<StockEnvelopeDTO> Handle(DetailsQuery message, CancellationToken cancellationToken)
        {
            var stocks = from s in _context.Stocks
                         where s.Articles.Any()
                            && s.StockId == message.StockId

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

            var stock = await stocks.FirstOrDefaultAsync();

            if (stock == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { StockDTO = Constants.NOT_FOUND });
            }

            return new StockEnvelopeDTO(stock);
        }
    }
}