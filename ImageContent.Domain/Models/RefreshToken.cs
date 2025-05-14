using ImageContent.Domain.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageContent.Domain.Models
{
    public class RefreshToken : BaseClass
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime? Revoked { get; set; }

        public bool IsActive => Revoked == null && !IsExpired;
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
