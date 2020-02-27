using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TradeTips.Security;

namespace TradeTips.Features.Comments
{
    [Route("articles")]
    public class CommentsController : Controller
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{id}/comments")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Creates a comment on an article",
            Description = "Creates an article comment in the repository",
            OperationId = "CreateComment",
            Tags = new[] { "Articles" }
        )]
        public async Task<CommentEnvelopeDTO> Create(int id, [FromBody]CreateCommand command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        [HttpGet("{id}/comments")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Gets a list of comments for an article",
            Description = "Retrieves a list of comments in the repository",
            OperationId = "GetComments",
            Tags = new[] { "Articles" }
        )]
        public async Task<CommentsEnvelopeDTO> Get(int id)
        {
            return await _mediator.Send(new ListQuery(id));
        }

        [HttpDelete("{articleId}/comments/{commentId}")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Removes a comment from an article",
            Description = "Deletes an article comment from the repository",
            OperationId = "DeleteComment",
            Tags = new[] { "Articles" }
        )]
        public async Task Delete(int articleId, int commentId)
        {
            await _mediator.Send(new DeleteCommand(articleId, commentId));
        }
    }
}