using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Common.Exceptions;
using Downloader.Common.Contracts;
using Downloader.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Front.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Front.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly IDownloaderProvider _downloaderProvider;
        private readonly IDownloaderManager _downloaderManager;

        public HomeController(
            IDownloaderProvider downloaderProvider,
            IDownloaderManager downloaderManager)
        {
            _downloaderProvider = downloaderProvider;
            _downloaderManager = downloaderManager;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return RedirectToAction("ShowRssSources");
        }

        [HttpGet]
        [Route("rss-source")]
        public async Task<IActionResult> ShowRssSourcesAsync()
        {
            var rssSources = await _downloaderProvider.GetAsync().ConfigureAwait(false);
            return View("RssSources", rssSources);
        }

        [HttpPost]
        [Route("rss-source/create")]
        public async Task<IActionResult> AddRssSourceAsync(RssSourceManageModel rssSource)
        {
            try
            {
                await _downloaderManager.CreateAsync(rssSource).ConfigureAwait(false);
            }
            catch (BaseHttpException e)
            {
                ModelState.AddModelError("Url", "Not a valid rss source");
            }

            return RedirectToAction("ShowRssSources");
        }

        [HttpPost]
        [Route("rss-source/update")]
        public async Task<IActionResult> UpdateRssSourceAsync(RssSourceManageModel rssSource)
        {
            try
            {
                await _downloaderManager.UpdateAsync(rssSource).ConfigureAwait(false);
            }
            catch (BaseHttpException _)
            {
                var rssSources = await _downloaderProvider.GetAsync().ConfigureAwait(false);
                ModelState.AddModelError<RssSourceManageModel>(s => s.Url, "Not a valid rss source");
                return View("RssSources", rssSources);
            }
            return RedirectToAction("ShowRssSources");
        }

        [HttpPost]
        [Route("rss-source/{rssSourceGuid:guid}/delete")]
        public async Task<IActionResult> DeleteRssSourceAsync([FromRoute] Guid rssSourceGuid)
        {
            await _downloaderManager.DeleteAsync(rssSourceGuid).ConfigureAwait(false);
            return RedirectToAction("ShowRssSources");
        }

        [HttpGet]
        [Route("news")]
        public async Task<IActionResult> ShowNewsAsync()
        {
            var news = await _downloaderManager.DownloadAllNewsAsync().ConfigureAwait(false);
            return View("News", news);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
