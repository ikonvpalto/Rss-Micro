using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common.Models;
using Manager.Common.Contracts;
using Manager.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Manager.API.Controllers
{
    [ApiController]
    [Route("api/manager")]
    public sealed class ManagerController : ControllerBase
    {
        private readonly IManagerProvider _managerProvider;
        private readonly IManagerManager _managerManager;

        public ManagerController(
            IManagerProvider managerProvider,
            IManagerManager managerManager)
        {
            _managerProvider = managerProvider;
            _managerManager = managerManager;
        }

        [HttpGet]
        [Route("{jobGuid:guid}")]
        [ProducesResponseType(typeof(JobModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid jobGuid)
        {
            var model = await _managerProvider.GetAsync(jobGuid).ConfigureAwait(false);
            return Ok(model);
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<JobModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync()
        {
            var models = await _managerProvider.GetAsync().ConfigureAwait(false);
            return Ok(models);
        }

        [HttpGet]
        [Route("ensure-valid")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> EnsureRssSourceValidAsync([FromQuery] string periodicity)
        {
            await _managerProvider.EnsureJobPeriodicityIsValidAsync(periodicity).ConfigureAwait(false);
            return Ok();
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] JobModel model)
        {
            await _managerManager.CreateAsync(model).ConfigureAwait(false);
            return Ok();
        }

        [HttpPut]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromBody] JobModel model)
        {
            await _managerManager.UpdateAsync(model).ConfigureAwait(false);
            return Ok();
        }

        [HttpDelete]
        [Route("{jobGuid:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid jobGuid)
        {
            await _managerManager.DeleteAsync(jobGuid).ConfigureAwait(false);
            return Ok();
        }
    }
}
