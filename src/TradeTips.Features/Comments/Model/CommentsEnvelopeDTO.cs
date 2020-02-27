using System.Collections.Generic;
using TradeTips.Domain;

namespace TradeTips.Features.Comments
{
    public class CommentsEnvelopeDTO
    {
        public CommentsEnvelopeDTO(List<CommentDTO> comments)
        {
            Comments = comments;
        }

        public List<CommentDTO> Comments { get; }
    }
}