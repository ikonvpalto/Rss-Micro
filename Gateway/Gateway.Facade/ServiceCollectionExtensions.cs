using System;
using System.Net.Http;
using Gateway.Common.Contracts;
using Gateway.Facade.HttpProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway.Facade
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGatewayProxies(this IServiceCollection services, string baseUrl)
        {
            services.AddScoped<IRssServiceProvider>(s =>
            {
                var httpClient = s.GetRequiredService<IHttpClientFactory>().CreateClient();
                httpClient.BaseAddress = new (baseUrl, UriKind.Absolute);
                return new RssServiceProviderProxy(httpClient);
            });

            services.AddScoped<IRssServiceManager>(s =>
            {
                var httpClient = s.GetRequiredService<IHttpClientFactory>().CreateClient();
                httpClient.BaseAddress = new (baseUrl, UriKind.Absolute);
                return new RssServiceManagerProxy(httpClient);
            });

            return services;
        }
    }
}
