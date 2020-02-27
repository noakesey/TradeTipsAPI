using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Features.Users;
using TradeTips.Domain;
using TradeTips.Security;

namespace TradeTips.Features.Users
{
    public class LoginHandler : IRequestHandler<LoginCommand, UserEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public LoginHandler(TradeTipsContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<UserEnvelopeDTO> Handle(LoginCommand message, CancellationToken cancellationToken)
        {
            var person = await _context.Persons.Where(x => x.Email == message.User.Email).SingleOrDefaultAsync(cancellationToken);
            if (person == null)
            {
                throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
            }

            if (!person.Hash.SequenceEqual(_passwordHasher.Hash(message.User.Password, person.Salt)))
            {
                throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
            }

            var user = _mapper.Map<TradeTips.Domain.Person, TradeTips.Features.Users.UserDTO>(person);
            user.Token = await _jwtTokenGenerator.CreateToken(person.Username);
            return new UserEnvelopeDTO(user);
        }
    }
}
