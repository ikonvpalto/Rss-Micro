using Api.Common;
using Downloader.Facade;
using Gateway.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway
{
    public sealed class Startup : BaseStartup
    {
        private const string DownloaderAddressParam = "DownloaderAddress";

        public Startup(IConfiguration configuration) : base(configuration) {}

        protected override IServiceCollection RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IRssSubscriptionProvider, RssSubscriptionProvider>();
            services.AddScoped<IRssSubscriptionManager, RssSubscriptionManager>();

            services.AddDownloaderProxy(Configuration[DownloaderAddressParam]);

            return services;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            BaseConfigure(app, env);
        }
    }
}
