using AutoMapper;
using ImageContent.Common.DTOs;
using ImageContent.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageContent.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddUserDto , ApplicationUser>().ReverseMap();
            CreateMap<AddUserDto , UserDto>().ReverseMap();
            CreateMap<ApplicationUser , UserDto>().ReverseMap();
        }
    }
}
