using TradeTips.Domain;

namespace TradeTips.Features.Articles
{
    public class ArticleEnvelopeDTO
    {
        public ArticleEnvelopeDTO(ArticleEditDTO article)
        {
            Article = article;
        }

        public ArticleEditDTO Article { get; }
    }
}