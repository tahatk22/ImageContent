using ImageContent.Domain.Models;
using ImageContent.Infrastracture.Database;
using Microsoft.AspNetCore.Identity;

namespace ImageContent.Extensions
{
    public static class IdentityExtension
    {
        public static void AddIdentity(this IServiceCollection services)
        {
            // Add Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();
        }
    }
}
