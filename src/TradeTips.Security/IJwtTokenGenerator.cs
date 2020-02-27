using System.Threading.Tasks;

namespace TradeTips.Security
{
    public interface IJwtTokenGenerator
    {
        Task<string> CreateToken(string username);
    }
}