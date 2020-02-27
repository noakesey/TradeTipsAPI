namespace TradeTips.Features.Profiles
{
    public class ProfileEnvelopeDTO
    {
        public ProfileEnvelopeDTO(ProfileDTO profile)
        {
            Profile = profile;
        }

        public ProfileDTO Profile { get; set; }
    }
}