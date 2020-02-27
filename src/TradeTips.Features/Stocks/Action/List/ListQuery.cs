using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Stocks
{
    public class ListQuery : IRequest<StocksEnvelopeDTO>
    {
        public ListQuery(int? limit, int? offset)
        {
            Limit = limit;
            Offset = offset;
        }

        public int? Limit { get; }
        public int? Offset { get; }
    }
}
