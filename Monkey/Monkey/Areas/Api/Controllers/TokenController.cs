using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monkey.Auth.Interfaces;
using Monkey.Core.Models.Auth;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.Tasks;

namespace Monkey.Areas.Api.Controllers
{
    [Route(EndPoint)]
    public class TokenController : ApiController
    {
        private const string EndPoint = AreaName + "/token";

        private readonly IAuthenticationService _authenticationService;

        public TokenController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        ///     Token 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AccessTokenModel))]
        public async Task<IActionResult> Token([FromBody]  RequestTokenModel model)
        {
            var accessToken = await _authenticationService.SignInAsync(model).ConfigureAwait(false);
            return Ok(accessToken);
        }
    }
}