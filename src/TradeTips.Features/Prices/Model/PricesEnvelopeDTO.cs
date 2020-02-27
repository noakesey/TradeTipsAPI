using System.Collections.Generic;
using TradeTips.Domain;

namespace TradeTips.Features.Prices
{
    public class PricesEnvelopeDTO
    {
        public string StockId { get; set; }
        public List<PriceDTO> Prices { get; set; }

        public int Count { get; set; }
    }
}