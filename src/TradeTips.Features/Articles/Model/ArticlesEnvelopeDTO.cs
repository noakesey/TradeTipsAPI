using System.Collections.Generic;
using TradeTips.Domain;

namespace TradeTips.Features.Articles
{
    public class ArticlesEnvelopeDTO
    {
        public List<ArticleListDTO> Articles { get; set; }

        public int ArticlesCount { get; set; }
    }
}