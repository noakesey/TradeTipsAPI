using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;

namespace TradeTips.Features.Stocks
{
    public class EditHandler : IRequestHandler<EditCommand, StockEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly IMapper _mapper;

        public EditHandler(TradeTipsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<StockEnvelopeDTO> Handle(EditCommand message, CancellationToken cancellationToken)
        {
            var stock = await _context.Stocks.Where(x => x.StockId == message.Stock.StockId)
                .FirstOrDefaultAsync(cancellationToken);

            if (stock == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { StockDTO = Constants.NOT_FOUND });
            }

            _mapper.Map(message.Stock, stock);

            await _context.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<Domain.Stock, StockDetailDTO>(stock);

            return new StockEnvelopeDTO(dto);
        }
    }
}
