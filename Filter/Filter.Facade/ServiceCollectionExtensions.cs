using System;
using System.Net.Http;
using Filter.Common.Contracts;
using Filter.Facade.HttpProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Filter.Facade
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFilterProxies(this IServiceCollection services, string baseUrl)
        {
            services.AddScoped<IFilterProvider>(s =>
            {
                var httpClient = s.GetRequiredService<IHttpClientFactory>().CreateClient();
                httpClient.BaseAddress = new (baseUrl, UriKind.Absolute);
                return new FilterProviderProxy(httpClient);
            });

            services.AddScoped<IFilterManager>(s =>
            {
                var httpClient = s.GetRequiredService<IHttpClientFactory>().CreateClient();
                httpClient.BaseAddress = new (baseUrl, UriKind.Absolute);
                return new FilterManagerProxy(httpClient);
            });

            return services;
        }
    }
}
