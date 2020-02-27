using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Comments
{
    public class ListQuery : IRequest<CommentsEnvelopeDTO>
    {
        public ListQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
