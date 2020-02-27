using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TradeTips.Security;

namespace TradeTips.Features.Articles
{
    [Route("articles")]
    public class ArticlesController : Controller
    {
        private readonly IMediator _mediator;

        public ArticlesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Gets a list of articles",
            Description = "Retrieves a list of articles from the repository",
            OperationId = "GetArticleList",
            Tags = new[] { "Articles" }
        )]
        public async Task<ArticlesEnvelopeDTO> Get([FromQuery] string tag, [FromQuery] string stockId, [FromQuery] string favorited, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new ListQuery(tag, stockId, favorited, limit, offset));
        }

        [HttpGet("feed")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Gets an article feed",
            Description = "Retrieves an article feed from the repository",
            OperationId = "GetArticleFeed",
            Tags = new[] { "Articles" }
        )]
        public async Task<ArticlesEnvelopeDTO> GetFeed([FromQuery] string tag, [FromQuery] string author, [FromQuery] string favorited, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new ListQuery(tag, author, favorited, limit, offset)
            {
                IsFeed = true
            });
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Gets an article",
            Description = "Retrieves an article from the repository",
            OperationId = "GetArticle",
            Tags = new[] { "Articles" }
        )]
        public async Task<ArticleEnvelopeDTO> Get(int id)
        {
            return await _mediator.Send(new DetailsQuery(id));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Creates a new article",
            Description = "Adds a new article to the repository",
            OperationId = "CreateArticle",
            Tags = new[] { "Articles" }
        )]
        public async Task<ArticleEnvelopeDTO> Create([FromBody] CreateCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Edits an article",
            Description = "Updates an article in the repository",
            OperationId = "EditArticle",
            Tags = new[] { "Articles" }
        )]
        public async Task<ArticleEnvelopeDTO> Edit(int id, [FromBody]EditCommand command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Deletes an article",
            Description = "Deletes an article from the repository",
            OperationId = "DeleteArticle",
            Tags = new[] { "Articles" }
        )]
        public async Task Delete(int id)
        {
            await _mediator.Send(new DeleteCommand(id));
        }
    }
}