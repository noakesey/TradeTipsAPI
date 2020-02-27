using MediatR;


namespace TradeTips.Features.Articles
{
    public class CreateCommand : IRequest<ArticleEnvelopeDTO>
    {
        public ArticleEditDTO Article { get; set; }
    }
}
