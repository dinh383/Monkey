using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monkey.Auth;
using Monkey.Auth.Helpers;
using Monkey.Auth.Interfaces;
using Monkey.Core.Models.Auth;
using Puppy.AutoMapper;
using System.Threading.Tasks;

namespace Monkey.Areas.Portal.Controllers
{
    [Route(AreaName + "/auth")]
    public class AuthController : MvcController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [Route("")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(new LoginModel());
        }

        [Route("")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Auth(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            RequestTokenModel requestToken = model.MapTo<RequestTokenModel>();
            requestToken.ClientId = AuthConfig.SystemClientId;
            requestToken.ClientSecret = AuthConfig.SystemClientSecret;

            AccessTokenModel accessToken = await _authenticationService.GetTokenAsync(requestToken).ConfigureAwait(true);

            TokenHelper.SetAccessTokenToCookie(Response.Cookies, accessToken);

            return View("Index", model);
        }
    }
}