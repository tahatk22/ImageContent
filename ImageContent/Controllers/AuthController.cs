using ImageContent.Common.DTOs;
using ImageContent.Common.Interfaces.IService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageContent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthController(IAuthService authService , IHttpContextAccessor httpContextAccessor)
        {
            this.authService = authService;
            this.httpContextAccessor = httpContextAccessor;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var loginUser = await authService.LoginAsync(loginDto);
            if (!loginUser.Success)
            {
                return BadRequest(loginUser.Error);
            }
            return Ok(loginUser);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var logoutUser = await authService.Logout();
            if (!logoutUser.Success)
            {
                return BadRequest(logoutUser.Error);
            }
            return Ok(logoutUser);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> refresh([FromBody] string refreshToken)
        {
            var refresh = await authService.RevokeRefreshToken(refreshToken);
            if (!refresh.Success)
            {
                return Unauthorized(refresh.Error);
            }
            return Ok(refresh);
        }
    }
}
