using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Stocks
{
    public class EditCommand : IRequest<StockEnvelopeDTO>
    {
        public StockSummaryDTO Stock { get; set; }
    }
}
