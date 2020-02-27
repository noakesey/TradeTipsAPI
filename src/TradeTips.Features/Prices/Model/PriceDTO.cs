using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Prices
{
    public class PriceDTO
    {
        public DateTime TransDate { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public long Volume { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
    }
}
