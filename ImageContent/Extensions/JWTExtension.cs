using ImageContent.Common.Interfaces.IService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ImageContent.Extensions
{
    public static class JWTExtension
    {
        public static void AddJwtBearerAuthentication(this IServiceCollection services , string issuer , string audience , string key)
        {
            // Add JWT Authentication
            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key)),
                    ClockSkew = TimeSpan.Zero
                };
                opt.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var tokenBlacklistService = context.HttpContext.RequestServices
                            .GetRequiredService<ITokenBlackListService>();

                        // Get the token from the request
                        var tokenString = context.HttpContext.Request.Headers["Authorization"]
                            .ToString().Replace("Bearer ", "");

                        // Check if token is blacklisted
                        if (tokenBlacklistService.IsTokenBlacklisted(tokenString))
                        {
                            context.Fail("Token has been revoked");
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
