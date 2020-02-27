using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TradeTips.Features.Users;

namespace TradeTips.Features.Users
{
    public class LoginCommand : IRequest<UserEnvelopeDTO>
    {
        public LoginUserDTO User { get; set; }
    }
}
