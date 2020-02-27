using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Stocks
{
    public class StockDetailDTO : StockSummaryDTO
    {
        public DateTime FirstPublicationDate { get; set; }
        public DateTime LatestPublicationDate { get; set; }
        public DateTime LatestDailyPriceDate { get; set; }
        public bool Obsolete { get; set; }
    }
}
