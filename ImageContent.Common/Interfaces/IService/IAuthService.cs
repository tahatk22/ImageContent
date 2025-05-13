using ImageContent.Common.BaseResponse;
using ImageContent.Common.DTOs;
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
    public interface IAuthService
    {
        Task<BaseCommandResponse<List<UserDto>>> GetAllUsersAsync(Expression<Func<ApplicationUser, bool>>? filter = null, string? props = null);
        Task<BaseCommandResponse<ApplicationUser>> GetUserAsync(Expression<Func<ApplicationUser, bool>> filter, string? props = null);
        Task<BaseCommandResponse<UserDto>> RegisterAsync(AddUserDto userDto);
        Task<BaseCommandResponse<ResponseAuth>> LoginAsync(LoginDto loginDto);
        Task<BaseCommandResponse<ApplicationUser>> Delete(ApplicationUser user);
        Task<BaseCommandResponse<ApplicationUser>> Update(ApplicationUser user);
        Task<BaseCommandResponse<UserDto>> Logout();
    }
}
