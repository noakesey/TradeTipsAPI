using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Comments
{
    public class DeleteCommand : IRequest
    {
        public DeleteCommand(int id, int id2)
        {
            Id = id;
            Id2 = id2;
        }

        public int Id { get; }
        public int Id2 { get; }
    }
}
