using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Stocks
{
    public class DeleteCommand : IRequest
    {
        public DeleteCommand(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
