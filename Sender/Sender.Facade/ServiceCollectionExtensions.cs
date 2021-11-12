using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Sender.Common.Contracts;
using Sender.Facade.HttpProxy;

namespace Sender.Facade
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSenderProxies(this IServiceCollection services, string baseUrl)
        {
            services.AddScoped<ISenderProvider>(s =>
            {
                var httpClient = s.GetService<HttpClient>();
                return new SenderProviderProxy(httpClient, baseUrl);
            });

            services.AddScoped<ISenderManager>(s =>
            {
                var httpClient = s.GetService<HttpClient>();
                return new SenderManagerProxy(httpClient, baseUrl);
            });

            return services;
        }
    }
}
