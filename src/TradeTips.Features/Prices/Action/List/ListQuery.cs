using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Prices
{
    public class ListQuery : IRequest<PricesEnvelopeDTO>
    {
        public ListQuery(string stockId, bool intraDay)
        {
            StockId = stockId;
            IntraDay = intraDay;
        }

        public string StockId { get; }

        public bool IntraDay { get; }
    }
}
