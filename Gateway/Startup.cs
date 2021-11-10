using Api.Common;
using Downloader.Facade;
using Filter.Facade;
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
        private const string FilterAddressParam = "FilterAddress";

        public Startup(IConfiguration configuration) : base(configuration) {}

        protected override IServiceCollection RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IRssSubscriptionProvider, RssSubscriptionProvider>();
            services.AddScoped<IRssSubscriptionManager, RssSubscriptionManager>();

            services.AddDownloaderProxies(Configuration[DownloaderAddressParam]);
            services.AddFilterProxies(Configuration[FilterAddressParam]);

            return services;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            BaseConfigure(app, env);
        }
    }
}
