using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;
using TradeTips.Security;

namespace TradeTips.Features.Tags
{
    [Route("tags")]
    public class TagsController : Controller
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Gets a list of tags",
            Description = "Retrieves a list of tags from the repository",
            OperationId = "GetTags",
            Tags = new[] { "Tags" }
        )]
        public async Task<TagsEnvelopeDTO> Get()
        {
            return await _mediator.Send(new ListQuery());
        }
    }
}