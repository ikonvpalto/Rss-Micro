using Api.Common;
using Downloader.API.Database;
using Downloader.API.ExternalRepositories;
using Downloader.API.Repositories;
using Downloader.API.Services;
using Downloader.Common.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Downloader.API
{
    public sealed class Startup : BaseStartup
    {
        public Startup(IConfiguration configuration) : base(configuration) {}

        protected override IServiceCollection RegisterServices(IServiceCollection services)
        {
            services.AddDbContext<DownloaderDbContext>(options =>
                options.UseNpgsql(Configuration["ConnectionString"]));

            services.AddScoped<IRssSourceRepository, RssSourceRepository>();
            services.AddScoped<IRssExternalRepository, RssExternalRepository>();
            services.AddScoped<IDownloaderManager, DownloaderManager>();
            services.AddScoped<IDownloaderProvider, DownloaderProvider>();

            return services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DownloaderDbContext dbContext)
        {
            BaseConfigure(app, env);

            dbContext.Database.Migrate();
        }
    }
}
