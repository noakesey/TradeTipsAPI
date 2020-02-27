using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TradeTips.Domain;
using TradeTips.Security;

namespace TradeTips.Features.Users
{
    public class EditHandler : IRequestHandler<EditCommand, UserEnvelopeDTO>
    {
        private readonly TradeTipsContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMapper _mapper;

        public EditHandler(TradeTipsContext context, IPasswordHasher passwordHasher,
            ICurrentUserAccessor currentUserAccessor, IMapper mapper)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _currentUserAccessor = currentUserAccessor;
            _mapper = mapper;
        }

        public async Task<UserEnvelopeDTO> Handle(EditCommand message, CancellationToken cancellationToken)
        {
            var currentUsername = _currentUserAccessor.GetCurrentUsername();
            var person = await _context.Persons.Where(x => x.Username == currentUsername).FirstOrDefaultAsync(cancellationToken);

            //TODO Ensure username doesn't already exist

            person.Username = message.User.Username ?? person.Username;
            person.Email = message.User.Email ?? person.Email;
            person.Bio = message.User.Bio ?? person.Bio;
            person.Image = message.User.Image ?? person.Image;

            if (!string.IsNullOrWhiteSpace(message.User.Password))
            {
                var salt = Guid.NewGuid().ToByteArray();
                person.Hash = _passwordHasher.Hash(message.User.Password, salt);
                person.Salt = salt;
            }

            await _context.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<TradeTips.Domain.Person, UserDTO>(person);

            return new UserEnvelopeDTO(dto);
        }
    }
}