using ImageContent.Common.Interfaces.IService;
using ImageContent.Common.BaseResponse;
using ImageContent.Common.DTOs;
using ImageContent.Common.FileHelper;
using ImageContent.Common.Interfaces.IRepository;
using ImageContent.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Claims;

namespace ImageContent.BL.Service
{
    public class DescriptiveImageService : IDescriptiveImageService
    {
        private readonly IRepository<DescriptiveImage> repository;
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor contextAccessor;

        public DescriptiveImageService(IRepository<DescriptiveImage> repository , HttpClient httpClient , IHttpContextAccessor contextAccessor)
        {
            this.repository = repository;
            this.httpClient = httpClient;
            this.contextAccessor = contextAccessor;
        }
        public async Task<BaseCommandResponse<DescriptiveImage>> AddAsync(IFormFile image)
        {
            var BaseResponse = new BaseCommandResponse<DescriptiveImage>();

            if (image.Length == 0 || image == null)
            {
                BaseResponse.Error = "You Must Insert Image";
                return BaseResponse;
            }

            try
            {
                // Here To Handle File Stream
                var ImageURL = ImageHelper.HandleImageURL(image);

                // Here To Handle Memory Stream
                using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();

                // Now Convert It To Base64
                var imageToBase64 = Convert.ToBase64String(imageBytes);

                var captionResponse = await GenerateCaptionAsync(image);

                var userId = contextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                var DescriptiveImage = new DescriptiveImage()
                {
                    CreatedUser = userId ?? string.Empty,
                    ImageURL = ImageURL,
                    Image = imageBytes,
                    ImageContent = imageToBase64,
                    Description = captionResponse.Caption
                };

                await repository.Add(DescriptiveImage);
                await repository.Save();
                BaseResponse.Error = string.Empty;
                BaseResponse.Message = "Added Successfully";
                return BaseResponse;
            }
            catch (Exception ex)
            {
                BaseResponse.Error = ex.Message;
                return BaseResponse;
            }
        }

        public async Task<BaseCommandResponse<DescriptiveImage>> Delete(DescriptiveImage image)
        {
            var BaseResponse = new BaseCommandResponse<DescriptiveImage>();
            try
            {
                repository.Delete(image);
                await repository.Save();
                BaseResponse.Error = string.Empty;
                BaseResponse.Message = "Deleted Successfully";
                return BaseResponse;
            }
            catch (Exception ex)
            {
                BaseResponse.Error = ex.Message;
                return BaseResponse;
            }
        }

        public async Task<BaseCommandResponse<IEnumerable<DescriptiveImage>>> GetAllImagesAsync(Expression<Func<DescriptiveImage, bool>>? filter, string? props = null)
        {
            var BaseResponse = new BaseCommandResponse<IEnumerable<DescriptiveImage>>();
            try
            {
                var Data = await repository.GetAll(filter, props);
                BaseResponse.Error = string.Empty;
                BaseResponse.Data = Data;
                return BaseResponse;
            }
            catch (Exception ex)
            {
                BaseResponse.Error = ex.Message;
                return BaseResponse;
            }
        }

        public async Task<BaseCommandResponse<DescriptiveImage>> GetImageAsync(Expression<Func<DescriptiveImage, bool>> filter, string? props = null)
        {
            var BaseResponse = new BaseCommandResponse<DescriptiveImage>();
            try
            {
                var Data = await repository.GetOne(filter, props);
                BaseResponse.Error = string.Empty;
                BaseResponse.Data = Data;
                return BaseResponse;
            }
            catch (Exception ex)
            {
                BaseResponse.Error = ex.Message;
                return BaseResponse;
            }
        }

        public async Task<BaseCommandResponse<DescriptiveImage>> Update(DescriptiveImage image)
        {
            var BaseResponse = new BaseCommandResponse<DescriptiveImage>();
            try
            {
                repository.Update(image);
                await repository.Save();
                BaseResponse.Error = string.Empty;
                BaseResponse.Message = "Updated Successfully";
                return BaseResponse;
            }
            catch (Exception ex)
            {
                BaseResponse.Error = ex.Message;
                return BaseResponse;
            }
        }

        public async Task<CaptionResponse> GenerateCaptionAsync(IFormFile image)
        {
            using var content = new MultipartFormDataContent();
            using var ms = new MemoryStream();
            await image.CopyToAsync(ms);
            ms.Position = 0;

            var imageContent = new StreamContent(ms);
            content.Add(imageContent, "file", image.FileName);

            var response = await httpClient.PostAsync("https://bambii-03-art-vision-art-description-generator.hf.space/generate_caption", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CaptionResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result!;
        }
    }
}
