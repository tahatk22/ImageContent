using ImageContent.Common.ValidationHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageContent.Common.DTOs
{
    public class AddImageDto
    {
        [AllowedExtensions(new string[] {".jpeg",".png"})]
        public IFormFile image { get; set; }
    }
}
