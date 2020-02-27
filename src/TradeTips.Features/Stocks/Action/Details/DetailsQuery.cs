using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Stocks
{
    public class DetailsQuery : IRequest<StockEnvelopeDTO>
    {
        public DetailsQuery(string stockId)
        {
            StockId = stockId;
        }

        public string StockId { get; set; }
    }
}
