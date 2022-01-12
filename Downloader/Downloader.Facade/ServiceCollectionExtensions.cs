using System;
using System.Net.Http;
using Downloader.Common.Contracts;
using Downloader.Facade.HttpProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Downloader.Facade
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDownloaderProxies(this IServiceCollection services, string baseUrl)
        {
            services.AddScoped<IDownloaderProvider>(s =>
            {
                var httpClient = s.GetRequiredService<IHttpClientFactory>().CreateClient();
                httpClient.BaseAddress = new (baseUrl, UriKind.Absolute);
                return new DownloadProviderProxy(httpClient);
            });

            services.AddScoped<IDownloaderManager>(s =>
            {
                var httpClient = s.GetRequiredService<IHttpClientFactory>().CreateClient();
                httpClient.BaseAddress = new (baseUrl, UriKind.Absolute);
                return new DownloadManagerProxy(httpClient);
            });

            return services;
        }
    }
}
