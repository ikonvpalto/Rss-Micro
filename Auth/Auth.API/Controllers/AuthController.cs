using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Auth.Common.Contracts;
using Auth.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [Route("login/url")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLoginUrlAsync()
        {
            var result = await _authService.GetLoginUrlAsync().ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet]
        [Route("logout/url")]
        [Authorize]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLogoutUrlAsync()
        {
            var result = await _authService.GetLogoutUrlAsync().ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet]
        [Route("profile")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfile), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProfileAsync()
        {
            var authToken = GetAuthToken();
            var result = await _authService.GetUserProfileAsync(authToken).ConfigureAwait(false);
            return Ok(result);
        }

        private string GetAuthToken()
        {
            return Request.Headers.Authorization
                .Single(a => a.Contains("Bearer", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
