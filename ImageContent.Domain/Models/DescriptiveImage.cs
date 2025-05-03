using ImageContent.Domain.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageContent.Domain.Models
{
    public class DescriptiveImage : BaseClass
    {
        public string? Description { get; set; }
        public string ImageURL { get; set; }
        public string? ImageContent { get; set; }
        public byte[]? Image { get; set; }
    }
}
