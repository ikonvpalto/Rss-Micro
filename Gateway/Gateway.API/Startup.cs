using Api.Common;
using Downloader.Facade;
using Filter.Facade;
using Gateway.Common.Contracts;
using Gateway.Common.Models;
using Gateway.Services;
using Manager.Facade;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Sender.Facade;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Gateway
{
    public sealed class Startup : BaseStartup
    {
        private const string DownloaderServiceName = "downloader-api";
        private const string FilterServiceName = "filter-api";
        private const string SenderServiceName = "sender-api";
        private const string ManagerServiceName = "manager-api";

        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override IServiceCollection RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IRssServiceProvider, RssServiceProvider>();
            services.AddScoped<IRssServiceManager, RssServiceManager>();

            services.AddDownloaderProxies(Configuration.GetServiceUri(DownloaderServiceName).ToString());
            services.AddFilterProxies(Configuration.GetServiceUri(FilterServiceName).ToString());
            services.AddSenderProxies(Configuration.GetServiceUri(SenderServiceName).ToString());
            services.AddManagerProxies(Configuration.GetServiceUri(ManagerServiceName).ToString());

            return services;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            BaseConfigure(app, env);
        }

        protected override void AddSwaggerDocs(SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.MapType<RssSubscription>(() => new()
            {
                Example =
                    new OpenApiObject
                    {
                        { "guid", new OpenApiString("3fa85f64-5717-4562-b3fc-2c963f66afa6") },
                        { "periodicity", new OpenApiString("*/5 * * * *") },
                        { "rssSource", new OpenApiString("https://onliner.by/feed") },
                        {
                            "filters", new OpenApiArray
                            {
                                new OpenApiString("covid")
                            }
                        },
                        {
                            "receivers", new OpenApiArray
                            {
                                new OpenApiString("example@gmail.com")
                            }
                        },
                    }
            });
            base.AddSwaggerDocs(swaggerGenOptions);
        }
    }
}
