using ImageContent.Common.Interfaces.IService;
using ImageContent.BL.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageContent.BL.ConfigureService
{
    public static class ServiceExtension
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddHttpClient<IDescriptiveImageService, DescriptiveImageService>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
