using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace TradeTips.Features.Profiles
{
    [Route("profiles")]
    public class ProfilesController : Controller
    {
        private readonly IMediator _mediator;

        public ProfilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{username}")]
        [SwaggerOperation(
            Summary = "Gets a user profile",
            Description = "Retrieves a user profile from the repository",
            OperationId = "GetProfile",
            Tags = new[] { "Profiles" }
        )]
        public async Task<ProfileEnvelopeDTO> Get(string username)
        {
            return await _mediator.Send(new Profiles.DetailsQuery()
            {
                Username = username
            });
        }
    }
}