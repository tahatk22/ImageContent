using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageContent.Common.FileHelper
{
    public static class ImageHelper
    {
        public static string HandleImageURL(IFormFile image)
        {
            try
            {
                var imagePath = CreateImageDirectory();
                var fileName = Guid.NewGuid() + Path.GetFileName(image.FileName);
                var FinalPath = Path.Combine(imagePath, fileName);
                using (var FileStream = new FileStream(FinalPath , FileMode.Create))
                {
                    image.CopyTo(FileStream);
                }
                return fileName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private static string CreateImageDirectory()
        {
            var imageDirectory = Path.Combine("Files","Images");
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }
            return imageDirectory;
        }
    }
}
