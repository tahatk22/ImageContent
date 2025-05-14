using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageContent.Common.DTOs
{
    public class LoginResponse : AuthResponse
    {
        public UserDto User { get; set; }
    }
}
