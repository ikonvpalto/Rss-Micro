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
                var httpClient = s.GetService<HttpClient>();
                return new FilterProviderProxy(httpClient, baseUrl);
            });

            services.AddScoped<IFilterManager>(s =>
            {
                var httpClient = s.GetService<HttpClient>();
                return new FilterManagerProxy(httpClient, baseUrl);
            });

            return services;
        }
    }
}
