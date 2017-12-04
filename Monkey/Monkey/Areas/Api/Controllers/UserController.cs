using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monkey.Auth.Interfaces;
using Monkey.Core.Models.Auth;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.Tasks;

namespace Monkey.Areas.Api.Controllers
{
    [Route(EndPoint)]
    public class UserController : ApiController
    {
        private const string EndPoint = AreaName + "/user";
        private const string AccessTokenEndPoint = "access-token";

        private readonly IAuthenticationService _authenticationService;

        public UserController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        ///     Access Token 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(AccessTokenEndPoint)]
        [AllowAnonymous]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AccessTokenModel))]
        public async Task<IActionResult> Token([FromBody]  RequestTokenModel model)
        {
            var accessToken = await _authenticationService.SignInAsync(HttpContext, model).ConfigureAwait(false);
            return Ok(accessToken);
        }
    }
}