using Api.Common;
using Downloader.Facade;
using Filter.Facade;
using Gateway.Models;
using Gateway.Services;
using Manager.Facade;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Sender.Facade;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Gateway
{
    public sealed class Startup : BaseStartup
    {
        private const string DownloaderAddressParam = "DownloaderAddress";
        private const string FilterAddressParam = "FilterAddress";
        private const string SenderAddressParam = "SenderAddress";
        private const string ManagerAddressParam = "ManagerAddress";

        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override IServiceCollection RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IRssSubscriptionProvider, RssSubscriptionProvider>();
            services.AddScoped<IRssSubscriptionManager, RssSubscriptionManager>();

            services.AddDownloaderProxies(Configuration[DownloaderAddressParam]);
            services.AddFilterProxies(Configuration[FilterAddressParam]);
            services.AddSenderProxies(Configuration[SenderAddressParam]);
            services.AddManagerProxies(Configuration[ManagerAddressParam]);

            return services;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            BaseConfigure(app, env);
        }

        protected override void AddSwaggerDocs(SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.MapType<RssSubscription>(() => new OpenApiSchema
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
            swaggerGenOptions.MapType<RssSubscriptionCreateModel>(() => new OpenApiSchema
            {
                Example =
                    new OpenApiObject
                    {
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
