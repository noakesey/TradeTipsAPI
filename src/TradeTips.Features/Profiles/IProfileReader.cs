using System.Threading.Tasks;

namespace TradeTips.Features.Profiles
{
    public interface IProfileReader
    {
        Task<ProfileEnvelopeDTO> ReadProfile(string username);
    }
}