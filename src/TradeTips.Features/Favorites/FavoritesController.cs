using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TradeTips.Features.Articles;
using TradeTips.Security;

namespace TradeTips.Features.Favorites
{
    [Route("articles")]
    public class FavoritesController : Controller
    {
        private readonly IMediator _mediator;

        public FavoritesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{id}/favorite")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Marks an article as a favourite",
            Description = "Creates a favourite in the repository",
            OperationId = "AddArticleFavourite",
            Tags = new[] { "Articles" }
        )]
        public async Task<ArticleEnvelopeDTO> FavoriteAdd(int id)
        {
            return await _mediator.Send(new AddCommand(id));
        }

        [HttpDelete("{id}/favorite")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Un-Marks an article as a favourite",
            Description = "Deletes a favourite from the repository",
            OperationId = "RemoveArticleFavourite",
            Tags = new[] { "Articles" }
        )]
        public async Task<ArticleEnvelopeDTO> FavoriteDelete(int id)
        {
            return await _mediator.Send(new DeleteCommand(id));
        }
    }
}