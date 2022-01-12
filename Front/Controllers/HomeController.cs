using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Front.Models;
using Front.ViewModels;
using Gateway.Common.Contracts;
using Gateway.Common.Models;

namespace Front.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private const string WelcomePageViewed = "WelcomePageViewed";

        private readonly IRssServiceProvider _rssServiceProvider;
        private readonly IRssServiceManager _rssServiceManager;
        private readonly IMapper _mapper;

        public HomeController(
            IRssServiceProvider rssServiceProvider,
            IRssServiceManager rssServiceManager, IMapper mapper)
        {
            _rssServiceProvider = rssServiceProvider;
            _rssServiceManager = rssServiceManager;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return IsWelcomePageViewed()
                ? RedirectToAction("ShowNews")
                : RedirectToAction("ShowAboutPage");
        }

        [HttpGet]
        [Route("rss-source")]
        public async Task<IActionResult> ShowRssSourcesAsync()
        {
            var rssSources = await _rssServiceProvider.GetSubscriptionsAsync().ConfigureAwait(false);
            var viewModels = _mapper.Map<ICollection<RssSubscriptionViewModel>>(rssSources);
            return View("RssSources", viewModels);
        }

        [HttpGet]
        [Route("about")]
        public IActionResult ShowAboutPageAsync()
        {
            return View("About");
        }

        [HttpGet]
        [Route("rss-source/create")]
        public IActionResult ShowRssSourceCreatePageAsync()
        {
            var rssSource = new RssSubscriptionViewModel { Guid = Guid.Empty };
            return View("RssSourceForm", rssSource);
        }

        [HttpGet]
        [Route("rss-source/{guid:guid}/update")]
        public async Task<IActionResult> ShowRssSourceUpdatePageAsync([FromRoute] Guid guid)
        {
            var rssSource = await _rssServiceProvider.GetSubscriptionAsync(guid);
            var viewModel = _mapper.Map<RssSubscriptionViewModel>(rssSource);
            return View("RssSourceForm", viewModel);
        }

        [HttpPost]
        [Route("rss-source")]
        public async Task<IActionResult> CreateOrUpdateRssSourceAsync([FromForm] RssSubscriptionViewModel rssSourceViewModel)
        {
            var model = _mapper.Map<RssSubscription>(rssSourceViewModel);
            try
            {
                await _rssServiceManager.CreateOrUpdateSubscriptionAsync(model).ConfigureAwait(false);
            }
            catch (BaseHttpException e)
            {
                ModelState.AddModelError(nameof(rssSourceViewModel.RssSource), "Not a valid rss source");
                return View("RssSourceForm", rssSourceViewModel);
            }

            return RedirectToAction("ShowRssSources");
        }

        [HttpPost]
        [Route("rss-source/{rssSourceGuid:guid}/delete")]
        public async Task<IActionResult> DeleteRssSourceAsync([FromRoute] Guid rssSourceGuid)
        {
            await _rssServiceManager.DeleteSubscriptionAsync(rssSourceGuid).ConfigureAwait(false);
            return RedirectToAction("ShowRssSources");
        }

        [HttpGet]
        [Route("news")]
        public async Task<IActionResult> ShowNewsAsync()
        {
            var news = await _rssServiceManager.DownloadNewsAsync().ConfigureAwait(false);
            return View("News", news);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool IsWelcomePageViewed()
            => HttpContext.Request.Cookies.ContainsKey(WelcomePageViewed);
    }
}
