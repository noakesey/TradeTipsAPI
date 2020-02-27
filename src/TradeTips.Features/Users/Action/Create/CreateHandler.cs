using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Features.Users;
using TradeTips.Domain;
using TradeTips.Security;

namespace TradeTips.Features.Users
{
    public class CreateHandler : IRequestHandler<CreateCommand, UserEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public CreateHandler(TradeTipsContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<UserEnvelopeDTO> Handle(CreateCommand message, CancellationToken cancellationToken)
        {
            if (await _context.Persons.Where(x => x.Username == message.User.Username).AnyAsync(cancellationToken))
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Username = Constants.IN_USE });
            }

            if (await _context.Persons.Where(x => x.Email == message.User.Email).AnyAsync(cancellationToken))
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Email = Constants.IN_USE });
            }

            var salt = Guid.NewGuid().ToByteArray();
            var person = new Person
            {
                Username = message.User.Username,
                Email = message.User.Email,
                Hash = _passwordHasher.Hash(message.User.Password, salt),
                Salt = salt
            };

            _context.Persons.Add(person);
            await _context.SaveChangesAsync(cancellationToken);
            var user = _mapper.Map<TradeTips.Domain.Person, TradeTips.Features.Users.UserDTO>(person);
            user.Token = await _jwtTokenGenerator.CreateToken(person.Username);
            return new UserEnvelopeDTO(user);
        }
    }
}
