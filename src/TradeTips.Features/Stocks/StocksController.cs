using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TradeTips.Security;

namespace TradeTips.Features.Stocks
{
    [Route("stocks")]
    public class StocksController
    {
        private readonly IMediator _mediator;

        public StocksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Creates a new stock",
            Description = "Adds a new stock to the repository",
            OperationId = "CreateStock",
            Tags = new[] { "Stocks" }
        )]
        public async Task<StockEnvelopeDTO> Create([FromBody] CreateCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet("{stockId}")]
        [SwaggerOperation(
            Summary = "Gets a stock",
            Description = "Retrieves a stock from the repository",
            OperationId = "GetStock",
            Tags = new[] { "Stocks" }
        )]
        public async Task<StockEnvelopeDTO> Get(string stockId)
        {
            return await _mediator.Send(new DetailsQuery(stockId));
        }

        [SwaggerOperation(
            Summary = "Retrieve a list of stocks",
            Description = "Retrieves a list of stocks from the repository",
            OperationId = "GetStocks",
            Tags = new[] { "Stocks" }
        )]
        [HttpGet]
        public async Task<StocksEnvelopeDTO> Get([FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new ListQuery(limit, offset));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Edits a stock",
            Description = "Updates a stock to the repository",
            OperationId = "UpdateStock",
            Tags = new[] { "Stocks" }
        )]
        public async Task<StockEnvelopeDTO> Edit(string id, [FromBody] EditCommand command)
        {
            command.Stock.StockId = id;
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        [SwaggerOperation(
            Summary = "Deletes a stock",
            Description = "Deletes a stock from the repository",
            OperationId = "DeleteStock",
            Tags = new[] { "Stocks" }
        )]
        public async Task Delete(string id)
        {
            await _mediator.Send(new DeleteCommand(id));
        }
    }
}