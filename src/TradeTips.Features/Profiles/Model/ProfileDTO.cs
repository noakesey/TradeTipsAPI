using Newtonsoft.Json;

namespace TradeTips.Features.Profiles
{
    public class ProfileDTO
    {
        public string Username { get; set; }

        public string Bio { get; set; }

        public string Image { get; set; }

        [JsonProperty("following")]
        public bool IsFollowed { get; set; }
    }
}