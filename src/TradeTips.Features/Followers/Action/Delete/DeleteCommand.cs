using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TradeTips.Features.Profiles;

namespace TradeTips.Features.Followers
{
    public class DeleteCommand : IRequest<ProfileEnvelopeDTO>
    {
        public DeleteCommand(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }
}
