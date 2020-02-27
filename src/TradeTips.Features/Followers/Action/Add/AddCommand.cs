using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TradeTips.Features.Profiles;

namespace TradeTips.Features.Followers
{
    public class AddCommand : IRequest<ProfileEnvelopeDTO>
    {
        public AddCommand(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }
}
