using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TradeTips.Features.Users;

namespace TradeTips.Features.Prices
{
    public class CreateCommand : IRequest<Stocks.StockEnvelopeDTO>
    {
        public string StockId { get; set; }

        public List<PriceDTO> Prices { get; set; }
    }
}
