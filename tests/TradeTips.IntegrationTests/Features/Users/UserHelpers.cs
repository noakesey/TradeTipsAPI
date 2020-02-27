using System.Threading.Tasks;
using TradeTips.Features.Users;

namespace TradeTips.IntegrationTests.Features.Users
{
    public static class UserHelpers
    {
        public static readonly string DefaultUserName = "username";

        /// <summary>
        /// creates a default user to be used in different tests
        /// </summary>
        /// <param name="fixture"></param>
        /// <returns></returns>
        public static async Task<UserDTO> CreateDefaultUser(SliceFixture fixture)
        {
            var command = new CreateCommand()
            {
                User = new CreateUserDTO()
                {
                    Email = "email",
                    Password = "password",
                    Username = DefaultUserName
                }
            };

            var commandResult = await fixture.SendAsync(command);
            return commandResult.User;
        }
    }
}
