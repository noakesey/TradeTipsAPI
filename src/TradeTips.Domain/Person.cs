using Newtonsoft.Json;
using System.Collections.Generic;

namespace TradeTips.Domain
{
    public class Person
    {
        public int PersonId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public string Image { get; set; }

        public List<ArticleFavorite> ArticleFavorites { get; set; }

        public List<FollowedPeople> Following { get; set; }

        public List<FollowedPeople> Followers { get; set; }

        public byte[] Hash { get; set; }

        public byte[] Salt { get; set; }
    }
}