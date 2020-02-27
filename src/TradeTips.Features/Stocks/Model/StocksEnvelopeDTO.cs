using System.Collections.Generic;
using TradeTips.Domain;

namespace TradeTips.Features.Stocks
{
    public class StocksEnvelopeDTO
    {
        public List<StockDetailDTO> Stocks { get; set; }

        public int StocksCount { get; set; }
    }
}