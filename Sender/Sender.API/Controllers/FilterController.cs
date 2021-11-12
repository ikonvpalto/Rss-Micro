using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Downloader.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Sender.Common.Contracts;
using Sender.Common.Models;

namespace Sender.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/sender")]
    public sealed class ReceiversController : ControllerBase
    {
        private readonly ISenderProvider _senderProvider;
        private readonly ISenderManager _senderManager;

        public ReceiversController(ISenderProvider senderProvider, ISenderManager senderManager)
        {
            _senderProvider = senderProvider;
            _senderManager = senderManager;
        }

        [HttpGet]
        [Route("{receiverGuid:guid}")]
        [ProducesResponseType(typeof(ReceiversModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid receiverGuid)
        {
            var receivers = await _senderProvider.GetAsync(receiverGuid).ConfigureAwait(false);
            return Ok(receivers);
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<ReceiversModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync()
        {
            var receivers = await _senderProvider.GetAsync().ConfigureAwait(false);
            return Ok(receivers);
        }

        [HttpPost]
        [Route("ensure-valid")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> EnsureReceiversValidAsync([FromBody] IEnumerable<string> receivers)
        {
            await _senderProvider.EnsureReceiversIsValidAsync(receivers).ConfigureAwait(false);
            return Ok();
        }

        [HttpPost]
        [Route("{receiverGuid:guid}/send-news")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SendNewsAsync([FromRoute] Guid receiverGuid, [FromBody] IEnumerable<NewsItem> news)
        {
            await _senderManager.SendNewsAsync(receiverGuid, news).ConfigureAwait(false);
            return Ok();
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateAsync([FromBody] ReceiversModel receivers)
        {
            await _senderManager.CreateAsync(receivers);
            return Ok();
        }

        [HttpPut]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] ReceiversModel receivers)
        {
            await _senderManager.UpdateAsync(receivers);
            return Ok();
        }

        [HttpDelete]
        [Route("{receiverGuid:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid receiverGuid)
        {
            await _senderManager.DeleteAsync(receiverGuid);
            return Ok();
        }
    }
}
