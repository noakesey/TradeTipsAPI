using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TradeTips.Domain
{
    public class DailyPrice
    {
        public int DailyPriceId { get; set; }
        public string StockId { get; set; }
        public DateTime TransDate { get; set; }

        [Column(TypeName = "decimal(12, 5)")]
        public decimal Open { get; set; }

        [Column(TypeName = "decimal(12, 5)")]
        public decimal High { get; set; }

        [Column(TypeName = "decimal(12, 5)")]
        public decimal Low { get; set; }

        [Column(TypeName = "decimal(12, 5)")]
        public decimal Close { get; set; }

        public long Volume { get; set; }
    }
}
