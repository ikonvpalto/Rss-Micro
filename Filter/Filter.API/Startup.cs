using Api.Common;
using Filter.API.Database;
using Filter.API.Repositories;
using Filter.API.Services;
using Filter.Common.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Filter.API
{
    public sealed class Startup : BaseStartup
    {
        public Startup(IConfiguration configuration) : base(configuration) {}

        protected override IServiceCollection RegisterServices(IServiceCollection services)
        {
            services.AddDbContext<FilterDbContext>(options =>
                options.UseNpgsql(Configuration["ConnectionString"]));

            services.AddScoped<IFilterRepository, FilterRepository>();
            services.AddScoped<IFilterProvider, FilterProvider>();
            services.AddScoped<IFilterManager, FilterManager>();

            return services;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, FilterDbContext dbContext)
        {
            BaseConfigure(app, env);

            dbContext.Database.Migrate();
        }
    }
}
