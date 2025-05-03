using ImageContent.Common.Interfaces.IService;
using ImageContent.Common.BaseResponse;
using ImageContent.Common.DTOs;
using ImageContent.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageContent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DescriptiveImageController : ControllerBase
    {
        private readonly IDescriptiveImageService descriptiveImage;

        public DescriptiveImageController(IDescriptiveImageService descriptiveImage)
        {
            this.descriptiveImage = descriptiveImage;
        }

        [HttpPost("AddImage")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddImage([FromForm] AddImageDto imageDto)
        {
            var addImageResult = await descriptiveImage.AddAsync(imageDto.image);
            if (!addImageResult.Success)
            {
                return BadRequest(addImageResult.Error);
            }
            return Ok(addImageResult);
        }
    }
}
