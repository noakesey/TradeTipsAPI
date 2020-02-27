using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Users
{
    public class EditCommand : IRequest<UserEnvelopeDTO>
    {
        public EditUserDTO User { get; set; }
    }
}
