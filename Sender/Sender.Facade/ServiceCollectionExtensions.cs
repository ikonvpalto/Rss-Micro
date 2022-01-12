using System;
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
                var httpClient = s.GetRequiredService<IHttpClientFactory>().CreateClient();
                httpClient.BaseAddress = new (baseUrl, UriKind.Absolute);
                return new SenderProviderProxy(httpClient);
            });

            services.AddScoped<ISenderManager>(s =>
            {
                var httpClient = s.GetRequiredService<IHttpClientFactory>().CreateClient();
                httpClient.BaseAddress = new (baseUrl, UriKind.Absolute);
                return new SenderManagerProxy(httpClient);
            });

            return services;
        }
    }
}
