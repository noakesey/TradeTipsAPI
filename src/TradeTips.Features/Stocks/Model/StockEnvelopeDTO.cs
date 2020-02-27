using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Stocks
{
    public class StockEnvelopeDTO
    {
        public StockEnvelopeDTO(StockDetailDTO stock)
        {
            Stock = stock;
        }

        public StockDetailDTO Stock { get; set; }
    }
}
