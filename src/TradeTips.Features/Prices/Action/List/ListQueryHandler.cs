using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;

namespace TradeTips.Features.Prices
{
    public class ListQueryHandler : IRequestHandler<ListQuery, PricesEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly IMapper _mapper;

        public ListQueryHandler(TradeTipsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PricesEnvelopeDTO> Handle(ListQuery message, CancellationToken cancellationToken)
        {
            List<PriceDTO> dtos;

            if (message.IntraDay)
            {
                var prices = await _context.IntraDayPrices
                    .Where(p => p.StockId == message.StockId)
                    .OrderBy(p => p.TransDate)
                    .ToListAsync();

                dtos = _mapper.Map<List<PriceDTO>>(prices);
            }
            else
            { 
                var prices = await _context.DailyPrices
                    .Where(p => p.StockId == message.StockId)
                    .OrderBy(p => p.TransDate)
                    .ToListAsync();

                dtos = _mapper.Map<List<PriceDTO>>(prices);
            }

            return new PricesEnvelopeDTO{
                Prices = dtos,
                Count = dtos.Count
            };
        }
    }
}