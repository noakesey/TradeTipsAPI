using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Users
{
    public class DetailsQuery : IRequest<UserEnvelopeDTO>
    {
        public string Username { get; set; }
    }
}
