using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;

namespace TradeTips.Features.Stocks
{
    public class CreateHandler : IRequestHandler<CreateCommand, StockEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly IMapper _mapper;

        public CreateHandler(TradeTipsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<StockEnvelopeDTO> Handle(CreateCommand message, CancellationToken cancellationToken)
        {
            if (await _context.Stocks.Where(x => x.StockId == message.Stock.StockId)
                .AnyAsync(cancellationToken))
            {
                throw new RestException(HttpStatusCode.BadRequest, new { StockDTO = Constants.IN_USE });
            }

            var stock = _mapper.Map<StockSummaryDTO, TradeTips.Domain.Stock>(message.Stock);

            _context.Stocks.Add(stock);

            await _context.SaveChangesAsync(cancellationToken);

            var newStock = _mapper.Map<TradeTips.Domain.Stock, StockDetailDTO>(stock);

            return new StockEnvelopeDTO(newStock);
        }
    }
}
