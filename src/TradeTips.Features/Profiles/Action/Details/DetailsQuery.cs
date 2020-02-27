using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Profiles
{
    public class DetailsQuery : IRequest<ProfileEnvelopeDTO>
    {
        public string Username { get; set; }
    }
}
