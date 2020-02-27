using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Articles
{
    public class DeleteCommand : IRequest
    {
        public DeleteCommand(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
