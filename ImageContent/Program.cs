
using ImageContent.Infrastracture.Database;
using Microsoft.EntityFrameworkCore;
using ImageContent.DAL.RepoInjection;
using ImageContent.BL.ConfigureService;
using ImageContent.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ImageContent.Common.Mapping;
using ImageContent.Common.SeedingHelper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using ImageContent.Common.Interfaces.IService;
using ImageContent.Extensions;
using Serilog;
using ImageContent.Common.Middleware;

namespace ImageContent
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Services
            builder.Services.AddControllers();

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });


            // Add Serilog
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext();
            });

            // Add JWT Authentication
            builder.Services.AddJwtBearerAuthentication( 
                builder.Configuration["Jwt:Issuer"],
                builder.Configuration["Jwt:Audience"],
                builder.Configuration["Jwt:Key"]);


            // Add HttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            // Add Seed Service
            builder.Services.AddScoped<IdentitySeeder>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add Database Context
            builder.Services.AddDbContext<AppDbContext>(op => 
            op.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Repositories and Services
            builder.Services.ConfigureRepos();
            builder.Services.ConfigureServices();

            // Add Identity
            builder.Services.AddIdentity();

            // Add AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));


            // Your Pipeline
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();
                await seeder.SeedRoles();
            }

            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                    diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
                };
            });

            app.UseMiddleware<RequestLoggingMiddleWare>();
            app.Run();
        }
    }
}
