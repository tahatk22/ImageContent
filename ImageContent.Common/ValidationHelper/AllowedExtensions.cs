using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageContent.Common.ValidationHelper
{
    public class AllowedExtensions : ValidationAttribute
    {
        private readonly string[] allowedExtensions;

        public AllowedExtensions(string[] allowedExtensions)
        {
            this.allowedExtensions = allowedExtensions;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            var fileExtension = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
            {
                return new ValidationResult($"This Extension Is Not Allowed And The Allowed Extensions is {string.Join(',', allowedExtensions)}");
            }
            return ValidationResult.Success;
        }


    }
}
