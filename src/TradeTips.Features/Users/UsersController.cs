using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TradeTips.Security;

namespace TradeTips.Features.Users
{
    [Route("users")]
    public class UsersController
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public UsersController(IMediator mediator, ICurrentUserAccessor currentUserAccessor)
        {
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a user",
            Description = "Adds a user to the repository",
            OperationId = "CreateUser",
            Tags = new[] { "Users" }
        )]
        public async Task<UserEnvelopeDTO> Create([FromBody] CreateCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Gets the current user",
            Description = "Retrieves the current user from the repository",
            OperationId = "GetCurrentUser",
            Tags = new[] { "Users" }
        )]
        public async Task<UserEnvelopeDTO> GetCurrent()
        {
            return await _mediator.Send(new DetailsQuery()
            {
                Username = _currentUserAccessor.GetCurrentUsername()
            });
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Updates a user",
            Description = "Updates a user in the repository",
            OperationId = "UpdateUser",
            Tags = new[] { "Users" }
        )]
        public async Task<UserEnvelopeDTO> UpdateUser([FromBody]EditCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Authenticates a user",
            Description = "Authenticates a user in the repository",
            OperationId = "Login",
            Tags = new[] { "Users" }
        )]
        public async Task<UserEnvelopeDTO> Login([FromBody] LoginCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}