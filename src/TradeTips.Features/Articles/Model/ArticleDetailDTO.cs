using System;

namespace TradeTips.Features.Articles
{
    public class ArticleDetailDTO : ArticleListDTO
    {
        public DateTime PreviousPublicationDate { get; set; }

        public double PreviousPrice { get; set; }

        public int ArticleCount { get; set; }

        
    }
}
