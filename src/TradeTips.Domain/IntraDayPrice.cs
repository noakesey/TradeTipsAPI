using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TradeTips.Domain
{
    public class IntraDayPrice
    {
        public int IntraDayPriceId { get; set; }
        public string StockId { get; set; }
        public DateTime TransDate { get; set; }

        [Column(TypeName = "decimal(12, 5)")]
        public decimal High { get; set; }

        [Column(TypeName = "decimal(12, 5)")]
        public decimal Low { get; set; }

        public long Volume { get; set; }
    }
}
