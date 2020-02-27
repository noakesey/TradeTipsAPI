using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Comments
{
    public class CreateCommand : IRequest<CommentEnvelopeDTO>
    {
        public CommentDTO Comment { get; set; }

        public int Id { get; set; }
    }
}
