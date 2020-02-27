using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TradeTips.Domain;
using TradeTips.Security;

namespace TradeTips.Features.Profiles
{
    public class ProfileReader : IProfileReader
    {
        private readonly TradeTipsContext _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMapper _mapper;

        public ProfileReader(TradeTipsContext context, ICurrentUserAccessor currentUserAccessor, IMapper mapper)
        {
            _context = context;
            _currentUserAccessor = currentUserAccessor;
            _mapper = mapper;
        }

        public async Task<ProfileEnvelopeDTO> ReadProfile(string username)
        {
            var currentUserName = _currentUserAccessor.GetCurrentUsername();

            var person = await _context.Persons.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == username);

            if (person == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { UserDTO = Constants.NOT_FOUND });
            }
            var profile = _mapper.Map<TradeTips.Domain.Person, ProfileDTO>(person);

            if (currentUserName != null)
            {
                var currentPerson = await _context.Persons
                    .Include(x => x.Following)
                    .Include(x => x.Followers)
                    .FirstOrDefaultAsync(x => x.Username == currentUserName);

                if (currentPerson.Followers.Any(x => x.TargetId == person.PersonId))
                {
                    profile.IsFollowed = true;
                }
            }
            return new ProfileEnvelopeDTO(profile);
        }
    }
}