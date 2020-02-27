using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TradeTips.Features.Articles;

namespace TradeTips.Features.Favorites
{
    public class DeleteCommand : IRequest<ArticleEnvelopeDTO>
    {
        public DeleteCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
