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

namespace ImageContent.BL.Service
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<ApplicationUser> applicationUser;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;

        public AuthService(IRepository<ApplicationUser> applicationUser , UserManager<ApplicationUser> userManager , 
            RoleManager<IdentityRole> roleManager , SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {
            this.applicationUser = applicationUser;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }
        public Task<BaseCommandResponse<ApplicationUser>> Delete(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseCommandResponse<List<UserDto>>> GetAllUsersAsync(Expression<Func<ApplicationUser, bool>>? filter = null, string? props = null)
        {
            var response = new BaseCommandResponse<List<UserDto>>();
            List<UserDto> users = new List<UserDto>();
            var applicationUsers =  await applicationUser.GetAll(filter, props);
            if (applicationUsers.Any())
            {
                foreach (var User in applicationUsers)
                {
                    var userDto = mapper.Map<UserDto>(User);
                    users.Add(userDto);
                }
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

        public Task<BaseCommandResponse<ApplicationUser>> LoginAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
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
    }
}
