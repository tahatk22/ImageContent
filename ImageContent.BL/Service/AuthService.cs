using ImageContent.Common.Interfaces.IService;
using ImageContent.Common.BaseResponse;
using ImageContent.Common.DTOs;
using ImageContent.Common.Interfaces.IRepository;
using ImageContent.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ImageContent.BL.Service
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<ApplicationUser> applicationUser;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITokenBlackListService tokenBlackListService;

        public AuthService(IRepository<ApplicationUser> applicationUser , UserManager<ApplicationUser> userManager , 
            RoleManager<IdentityRole> roleManager , SignInManager<ApplicationUser> signInManager,
            IMapper mapper , IConfiguration configuration , IHttpContextAccessor httpContextAccessor
            ,ITokenBlackListService tokenBlackListService)
        {
            this.applicationUser = applicationUser;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            this.tokenBlackListService = tokenBlackListService;
        }
        public Task<BaseCommandResponse<ApplicationUser>> Delete(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseCommandResponse<List<UserDto>>> GetAllUsersAsync(Expression<Func<ApplicationUser, bool>>? filter = null, string? props = null)
        {
            var response = new BaseCommandResponse<List<UserDto>>();
            var applicationUsers =  await applicationUser.GetAll(filter, props);
            if (applicationUsers.Any())
            {
                var users = mapper.Map<List<UserDto>>(applicationUsers);
                response.Data = users;
                response.Count = users.Count;
                return response;
            }
            else
            {
                response.Error = "There is No Users";
                return response;
            }
        }

        public Task<BaseCommandResponse<ApplicationUser>> GetUserAsync(Expression<Func<ApplicationUser, bool>> filter, string? props = null)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseCommandResponse<ResponseAuth>> LoginAsync(LoginDto loginDto)
        {
            var response = new BaseCommandResponse<ResponseAuth>();
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName);
            if (user is null)
            {
                response.Error = "There Is No User With This Name";
                return response;
            }
            var result = await signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
            if (result.Succeeded)
            {
                var jwtSecurityToken = await GenerateToken(user);
                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                tokenBlackListService.StoreUserTokens(user.Id, token);
                tokenBlackListService.BlackListPreviousToken(user.Id, token);
                var userDto = mapper.Map<UserDto>(user);
                var responseAuth = new ResponseAuth
                {
                    Token = token,
                    User = userDto
                };
                response.Message = $"User {loginDto.UserName} Loged In Successfuly";
                response.Data = responseAuth;
                return response;
            }
            else
            {
                response.Error = "Username Or Password Is Incorrect";
                return response;
            }
        }

        public async Task<BaseCommandResponse<UserDto>> Logout()
        {
            var response = new BaseCommandResponse<UserDto>();
            try
            {
                var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
                await signInManager.SignOutAsync();
                tokenBlackListService.BlacklistToken(token);
                response.Message = $"User {user?.FirstName} Logged Out Successfuly";
                return response;
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
                return response;
            }
        }

        public async Task<BaseCommandResponse<UserDto>> RegisterAsync(AddUserDto userDto)
        {
            var response = new BaseCommandResponse<UserDto>();

            // Check For Existing User
            var existingUser = await userManager.FindByEmailAsync(userDto.Email);
            if (existingUser is not null)
            {
                response.Error = "There Is User With This Email";
                return response;
            }
            var User = mapper.Map<ApplicationUser>(userDto);
            var UserDto = mapper.Map<UserDto>(userDto);
            var createUser = await userManager.CreateAsync(User , userDto.Password);
            if (createUser.Succeeded)
            {
                response.Message = $"User {userDto.UserName} Created Successfuly";
                response.Data = UserDto;
                return response;
            }
            else
            {
                response.Error = createUser.Errors.FirstOrDefault()?.Description ?? "Unknown error occurred.";
                return response;
            }
            
        }

        public Task<BaseCommandResponse<ApplicationUser>> Update(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            string userRoles = string.Join(",", roles);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("UserID", user.Id.ToString()),
                new Claim("role", userRoles)
        }
            .Union(userClaims);


            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:Key").Value));
            var signingCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration.GetSection("Jwt:Issuer").Value,
                audience: configuration.GetSection("Jwt:Audience").Value,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration.GetSection("Jwt:ExpireTime").Value)),
                signingCredentials: signingCredentials);

            return token;
        }
    }
}
