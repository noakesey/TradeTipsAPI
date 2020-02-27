using System.Collections.Generic;

namespace TradeTips.Domain
{
    public class Tag
    {
        public string TagId { get; set; }

        public List<ArticleTag> ArticleTags { get; set; }
    }
}