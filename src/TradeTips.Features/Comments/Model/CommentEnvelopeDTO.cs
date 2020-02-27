using TradeTips.Domain;

namespace TradeTips.Features.Comments
{
    public class CommentEnvelopeDTO
    {
        public CommentEnvelopeDTO(CommentDTO comment)
        {
            Comment = comment;
        }

        public CommentDTO Comment { get; }
    }
}