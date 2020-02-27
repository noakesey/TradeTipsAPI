using System;

namespace TradeTips.Features.Articles
{
    public class ArticleListDTO : ArticleEditDTO
    {
        public string Username { get; set; }

        public decimal PriceTMinus7 { get; set; }
        public decimal PriceTMinus1 { get; set; }
        public decimal PriceOpen { get; set; }
        public decimal PriceClose { get; set; }
        public decimal PriceTPlus1 { get; set; }
        public decimal PriceTPlus2 { get; set; }
        public decimal PriceTPlus3 { get; set; }
        public decimal PriceTPlus5 { get; set; }
        public decimal PriceTPlus7 { get; set; }
    }
}
