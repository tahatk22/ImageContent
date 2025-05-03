using ImageContent.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageContent.Infrastracture.Database
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
                
        }
        public DbSet<DescriptiveImage> descriptiveImages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DescriptiveImage>()
                .Property(x => x.Id)
                .HasConversion
                (
                    c => c.ToString(),
                    c => Ulid.Parse(c)
                ).HasColumnType("text");

            base.OnModelCreating(modelBuilder);
        }
    }
}
