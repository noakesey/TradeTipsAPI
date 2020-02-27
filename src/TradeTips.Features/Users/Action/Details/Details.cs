using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;
using TradeTips.Security;

namespace TradeTips.Features.Users
{
    public class QueryHandler : IRequestHandler<DetailsQuery, UserEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public QueryHandler(TradeTipsContext context, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
        {
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<UserEnvelopeDTO> Handle(DetailsQuery message, CancellationToken cancellationToken)
        {
            var person = await _context.Persons
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == message.Username, cancellationToken);

            if (person == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { UserDTO = Constants.NOT_FOUND });
            }

            var user = _mapper.Map<TradeTips.Domain.Person, UserDTO>(person);
            user.Token = await _jwtTokenGenerator.CreateToken(person.Username);

            return new UserEnvelopeDTO(user);
        }
    }
}
