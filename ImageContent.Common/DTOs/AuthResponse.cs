using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageContent.Common.DTOs
{
    public class AuthResponse
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }
}
