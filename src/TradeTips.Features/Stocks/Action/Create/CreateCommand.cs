using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TradeTips.Features.Users;

namespace TradeTips.Features.Stocks
{
    public class CreateCommand : IRequest<StockEnvelopeDTO>
    {
        public StockSummaryDTO Stock { get; set; }
    }
}
