using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Features.Profiles;
using TradeTips.Domain;
using TradeTips.Security;

namespace TradeTips.Features.Followers
{
    public class DeleteHandler : IRequestHandler<DeleteCommand, ProfileEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IProfileReader _profileReader;

        public DeleteHandler(TradeTipsContext context, ICurrentUserAccessor currentUserAccessor, IProfileReader profileReader)
        {
            _context = context;
            _currentUserAccessor = currentUserAccessor;
            _profileReader = profileReader;
        }

        public async Task<ProfileEnvelopeDTO> Handle(DeleteCommand message, CancellationToken cancellationToken)
        {
            var target = await _context.Persons.FirstOrDefaultAsync(x => x.Username == message.Username, cancellationToken);

            if (target == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { UserDTO = Constants.NOT_FOUND });
            }

            var observer = await _context.Persons.FirstOrDefaultAsync(x => x.Username == _currentUserAccessor.GetCurrentUsername(), cancellationToken);

            var followedPeople = await _context.FollowedPeople.FirstOrDefaultAsync(x => x.ObserverId == observer.PersonId && x.TargetId == target.PersonId, cancellationToken);

            if (followedPeople != null)
            {
                _context.FollowedPeople.Remove(followedPeople);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return await _profileReader.ReadProfile(message.Username);
        }
    }
}