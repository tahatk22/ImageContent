using ImageContent.Common.BaseResponse;
using ImageContent.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace ImageContent.Common.Interfaces.IService
{
    public interface IDescriptiveImageService
    {
        Task<BaseCommandResponse<IEnumerable<DescriptiveImage>>> GetAllImagesAsync(Expression<Func<DescriptiveImage,bool>>? filter , string? props = null);
        Task<BaseCommandResponse<DescriptiveImage>> GetImageAsync(Expression<Func<DescriptiveImage, bool>> filter , string? props = null);
        Task<BaseCommandResponse<DescriptiveImage>> AddAsync(IFormFile image);
        Task<BaseCommandResponse<DescriptiveImage>> Delete(DescriptiveImage image);
        Task<BaseCommandResponse<DescriptiveImage>> Update(DescriptiveImage image);
    }
}
