using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monkey.Auth.Interfaces;
using Monkey.Core.Models.Auth;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.Tasks;

namespace Monkey.Controllers.Api
{
    [Route(EndPoint)]
    public class TokenApiController : ApiController
    {
        private const string EndPoint = AreaName + "/token";

        private readonly IAuthenticationService _authenticationService;

        public TokenApiController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        ///     Token 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(AccessTokenModel), "Response access token data")]
        public async Task<IActionResult> Token([FromBody]  RequestTokenModel model)
        {
            var accessToken = await _authenticationService.SignInAsync(model).ConfigureAwait(false);
            return Ok(accessToken);
        }
    }
}