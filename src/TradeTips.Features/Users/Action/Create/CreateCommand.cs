using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TradeTips.Features.Users;

namespace TradeTips.Features.Users
{
    public class CreateCommand : IRequest<UserEnvelopeDTO>
    {
        public CreateUserDTO User { get; set; }
    }
}
