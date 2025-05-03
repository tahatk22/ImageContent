using ImageContent.Common.DTOs;
using ImageContent.Common.Interfaces.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageContent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> Register(AddUserDto addUserDto)
        {
            var createUser = await authService.RegisterAsync(addUserDto);
            if (!createUser.Success)
            {
                return BadRequest(createUser.Error);
            }
            return Ok(createUser);
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAll()
        {
            var getUsers = await authService.GetAllUsersAsync();
            if (!getUsers.Success)
            {
                return BadRequest(getUsers.Error);
            }
            return Ok(getUsers);
        }
    }
}
