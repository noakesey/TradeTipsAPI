using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TradeTips.Features.Profiles;
using TradeTips.Security;

namespace TradeTips.Features.Followers
{
    [Route("profiles")]
    public class FollowersController : Controller
    {
        private readonly IMediator _mediator;

        public FollowersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{username}/follow")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Follows a user profile",
            Description = "Creates a user subscription in the repository",
            OperationId = "FollowProfile",
            Tags = new[] { "Profiles" }
        )]
        public async Task<ProfileEnvelopeDTO> Follow(string username)
        {
            return await _mediator.Send(new AddCommand(username));
        }

        [HttpDelete("{username}/follow")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Un-follows a user profile",
            Description = "Deletes a user subscription from the repository",
            OperationId = "UnFollowProfile",
            Tags = new[] { "Profiles" }
        )]
        public async Task<ProfileEnvelopeDTO> Unfollow(string username)
        {
            return await _mediator.Send(new DeleteCommand(username));
        }
    }
}