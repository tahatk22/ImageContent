
using ImageContent.Infrastracture.Database;
using Microsoft.EntityFrameworkCore;
using ImageContent.DAL.RepoInjection;
using ImageContent.BL.ConfigureService;
using ImageContent.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ImageContent.Common.Mapping;

namespace ImageContent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Services
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(op => 
            op.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.ConfigureRepos();
            builder.Services.ConfigureServices();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();

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

            app.Run();
        }
    }
}
