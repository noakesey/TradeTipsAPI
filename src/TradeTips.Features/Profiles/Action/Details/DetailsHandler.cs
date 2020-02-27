using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace TradeTips.Features.Profiles
{
    public class DetailsHandler : IRequestHandler<DetailsQuery, ProfileEnvelopeDTO>
    {
        private readonly IProfileReader _profileReader;

        public DetailsHandler(IProfileReader profileReader)
        {
            _profileReader = profileReader;
        }

        public async Task<ProfileEnvelopeDTO> Handle(DetailsQuery message, CancellationToken cancellationToken)
        {
            return await _profileReader.ReadProfile(message.Username);
        }
    }
}