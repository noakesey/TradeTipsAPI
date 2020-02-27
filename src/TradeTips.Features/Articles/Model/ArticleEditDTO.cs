using System;

namespace TradeTips.Features.Articles
{
    public class ArticleEditDTO
    {
        public int ArticleId { get; set; }

        public string StockId { get; set; }

        public DateTime PublicationDate { get; set; }

        public string Title { get; set; }

        public string Teaser { get; set; }

        public string Summary { get; set; }

        public string Link { get; set; }

        public string StockLink { get; set; }

        public double Price { get; set; }

        public bool IsAlpha { get; set; }

        public string[] TagList { get; set; }
    }
}
