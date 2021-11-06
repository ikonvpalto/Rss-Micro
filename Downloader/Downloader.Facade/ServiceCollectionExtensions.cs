using System.Net.Http;
using Downloader.Common.Contracts;
using Downloader.Facade.HttpProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Downloader.Facade
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDownloaderProxy(this IServiceCollection services, string baseUrl)
        {
            services.AddScoped<IDownloaderProvider>(s =>
            {
                var httpClient = s.GetService<HttpClient>();
                return new DownloadProviderProxy(httpClient, baseUrl);
            });

            services.AddScoped<IDownloaderManager>(s =>
            {
                var httpClient = s.GetService<HttpClient>();
                return new DownloadManagerProxy(httpClient, baseUrl);
            });

            return services;
        }
    }
}
