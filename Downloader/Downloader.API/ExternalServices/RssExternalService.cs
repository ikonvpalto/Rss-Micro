using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Common.Exceptions;
using Common.Extensions;
using Downloader.API.ExternalServices;
using Downloader.API.Models;
using Downloader.API.Resources;
using Microsoft.Extensions.Logging;

namespace Downloader.API.ExternalRepositories
{
    public sealed class RssExternalService : IRssExternalService
    {
        private const string RssRootTag = "rss";
        private const string RssRootTagVersionAttribute = "version";
        private const string RssDescriptionElement = "description";
        private const string RssTitleElement = "title";
        private const string RssPublishDateElement = "pubDate";
        private const int RequiredRssVersion = 2;

        private readonly HttpClient _httpClient;
        private readonly ILogger<RssExternalService> _logger;

        public RssExternalService(HttpClient httpClient, ILogger<RssExternalService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task EnsureCorrectRssSourceAsync(string rssSourceUrl)
        {
            var (response, stringResponse) = await RequestAsync(rssSourceUrl).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(Localization.RssSourceRequestFailed, rssSourceUrl, response.StatusCode, response.ReasonPhrase, stringResponse);
                throw new BadRequestException(Localization.NotARssSource);
            }

            var xmlDocument = XDocument.Parse(stringResponse);
            if (!IsXmlIsCorrectRssDocument(xmlDocument))
            {
                throw new BadRequestException(Localization.NotARssSource);
            }
        }

        public async Task<IEnumerable<RssSourceResponseItem>> RequestRssSourceAsync(string rssSourceUrl)
        {
            var (response, stringResponse) = await RequestAsync(rssSourceUrl).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(Localization.RssSourceRequestFailed, rssSourceUrl, response.StatusCode, response.ReasonPhrase, stringResponse);
                throw new ServerInnerException(Localization.RssSourceNotAvailable);
            }

            var xmlDocument = XDocument.Parse(stringResponse);
            if (!IsXmlIsCorrectRssDocument(xmlDocument))
            {
                throw new BadRequestException(Localization.NotARssSource);
            }

            return xmlDocument.Root?.Element("channel")?.Elements("item").Select(i => new RssSourceResponseItem
                   {
                       Description = i.Element(RssDescriptionElement)?.Value ?? string.Empty,
                       Title = i.Element(RssTitleElement)?.Value ?? string.Empty,
                       PublishDate = DateTime.TryParse(i.Element(RssPublishDateElement)?.Value, out var date)
                           ? date
                           : DateTime.MinValue
                   })
                   ?? Array.Empty<RssSourceResponseItem>();
        }

        private static bool IsXmlIsCorrectRssDocument(XDocument xmlDocument)
        {
            var rootElement = xmlDocument?.Root;
            if (rootElement == null
                || !RssRootTag.Equals(rootElement.Name.LocalName)
                || !rootElement.HasAttributes)
            {
                return false;
            }

            var versionAttribute = rootElement.Attributes()
                .SingleOrDefault(a => RssRootTagVersionAttribute.Equals(a.Name.LocalName));
            if (versionAttribute == null
                || !double.TryParse(versionAttribute.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var version)
                || !version.ApproximatelyEquals(RequiredRssVersion))
            {
                return false;
            }

            return true;
        }

        private async Task<(HttpResponseMessage response, string stringResponse)> RequestAsync(string url)
        {
            var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            _logger.LogTrace("{0} responded with body:\n{1}", url, stringResponse);

            return (response, stringResponse);
        }
    }
}
