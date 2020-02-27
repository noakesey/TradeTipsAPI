using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Users
{
    public class UserEnvelopeDTO
    {
        public UserEnvelopeDTO(UserDTO user)
        {
            User = user;
        }

        public UserDTO User { get; set; }
    }
}
