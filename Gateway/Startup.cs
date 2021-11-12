using Api.Common;
using Downloader.Facade;
using Filter.Facade;
using Gateway.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sender.Facade;

namespace Gateway
{
    public sealed class Startup : BaseStartup
    {
        private const string DownloaderAddressParam = "DownloaderAddress";
        private const string FilterAddressParam = "FilterAddress";
        private const string SenderAddressParam = "SenderAddress";

        public Startup(IConfiguration configuration) : base(configuration) {}

        protected override IServiceCollection RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IRssSubscriptionProvider, RssSubscriptionProvider>();
            services.AddScoped<IRssSubscriptionManager, RssSubscriptionManager>();

            services.AddDownloaderProxies(Configuration[DownloaderAddressParam]);
            services.AddFilterProxies(Configuration[FilterAddressParam]);
            services.AddSenderProxies(Configuration[SenderAddressParam]);

            return services;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            BaseConfigure(app, env);
        }
    }
}
