using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTips.Features.Users
{
    public class EditUserDTO : CreateUserDTO
    {
        public string Bio { get; set; }

        public string Image { get; set; }
    }
}
