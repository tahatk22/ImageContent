using ImageContent.Common.Interfaces.IRepository;
using ImageContent.DAL.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageContent.DAL.RepoInjection
{
    public static class RepositoryInjection
    {
        public static void ConfigureRepos(this IServiceCollection services)
        {
            services.AddScoped((typeof(IRepository<>)), typeof(Repository<>));
        }
    }
}
