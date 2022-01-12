using System;
using System.Net.Http;
using Manager.Common.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Manager.Facade.HttpProxy;

namespace Manager.Facade
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddManagerProxies(this IServiceCollection services, string baseUrl)
        {
            services.AddScoped<IManagerProvider>(s =>
            {
                var httpClient = s.GetRequiredService<IHttpClientFactory>().CreateClient();
                httpClient.BaseAddress = new (baseUrl, UriKind.Absolute);
                return new ManagerProviderProxy(httpClient);
            });

            services.AddScoped<IManagerManager>(s =>
            {
                var httpClient = s.GetRequiredService<IHttpClientFactory>().CreateClient();
                httpClient.BaseAddress = new (baseUrl, UriKind.Absolute);
                return new ManagerManagerProxy(httpClient);
            });

            return services;
        }
    }
}
