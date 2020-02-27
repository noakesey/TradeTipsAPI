using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Articles
{
    public class EditCommand : IRequest<ArticleEnvelopeDTO>
    {
        public ArticleEditDTO Article { get; set; }
        public int Id { get; set; }
    }
}
