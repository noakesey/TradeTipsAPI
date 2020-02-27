using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TradeTips.Security;

namespace TradeTips.Features.Prices
{
    [Route("prices")]
    public class PricesController
    {
        private readonly IMediator _mediator;

        public PricesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Creates a new price",
            Description = "Adds a new price to the repository",
            OperationId = "CreatePrice",
            Tags = new[] { "Prices" }
        )]
        public async Task<Stocks.StockEnvelopeDTO> CreateDaily([FromBody] CreateCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet("{id}/daily")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Gets daily prices for a stock",
            Description = "Retrieves daily prices for a stock from the repository",
            OperationId = "GetDaily",
            Tags = new[] { "Prices" }
        )]
        public async Task<PricesEnvelopeDTO> GetDaily(string id)
        {
            return await _mediator.Send(new ListQuery(id, false));
        }

        [HttpGet("{id}/intraday")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Gets intraday prices for a stock",
            Description = "Retrieves intraday prices for a stock from the repository",
            OperationId = "GetDaily",
            Tags = new[] { "Prices" }
        )]
        public async Task<PricesEnvelopeDTO> GetIntraDay(string id)
        {
            return await _mediator.Send(new ListQuery(id, true));
        }
    }
}