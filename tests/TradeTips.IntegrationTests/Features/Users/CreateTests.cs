using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TradeTips.Features.Users;
using TradeTips.Security;
using Xunit;

namespace TradeTips.IntegrationTests.Features.Users
{
    public class CreateTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Create_User()
        {
            var command = new TradeTips.Features.Users.CreateCommand()
            {
                User = new CreateUserDTO()
                {
                    Email = "email",
                    Password = "password",
                    Username = "username"
                }
            };

            await SendAsync(command);

            var created = await ExecuteDbContextAsync(db => db.Persons.Where(d => d.Email == command.User.Email).SingleOrDefaultAsync());

            Assert.NotNull(created);
            Assert.Equal(created.Hash, new PasswordHasher().Hash("password", created.Salt));
        }
    }
}