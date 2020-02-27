using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Domain
{
    public class Stock
    {
        public string StockId { get; set; }
        public string Name { get; set; }
        public bool Obsolete { get; set; }
        public bool BargainShare { get; set; }

        public virtual List<Article> Articles { get; }
        public virtual List<DailyPrice> DailyPrices { get; }
        public virtual List<IntraDayPrice> IntraDayPrices { get; }
    }
}
