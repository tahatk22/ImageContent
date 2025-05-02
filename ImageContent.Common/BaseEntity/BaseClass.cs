using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ImageContent.Common.BaseEntity
{
    public abstract class BaseClass
    {
        public Ulid Id { get; set; } = Ulid.NewUlid();
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        [Required]
        public string CreatedUser { get; set; }
        public string? ModifiedUser { get; set; }
    }
}
