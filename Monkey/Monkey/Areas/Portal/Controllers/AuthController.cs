using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monkey.Auth;
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

        [Route("sign-in")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            RequestTokenModel requestToken = model.MapTo<RequestTokenModel>();

            // Update system client id and client secret
            requestToken.ClientId = AuthConfig.SystemClientId;
            requestToken.ClientSecret = AuthConfig.SystemClientSecret;

            // Sign In and get access token
            AccessTokenModel accessTokenModel = await _authenticationService.SignInAsync(requestToken).ConfigureAwait(true);

            // Sign In to Cookie, for web only
            await _authenticationService.SignInCookieAsync(Response.Cookies, accessTokenModel).ConfigureAwait(true);

            return RedirectToAction("Index", "Home");
        }

        [Route("sign-out")]
        [HttpGet]
        [Auth.Filters.Auth]
        public async Task<IActionResult> SignOut()
        {
            await _authenticationService.SignOutCookieAsync(Response.Cookies).ConfigureAwait(true);

            return RedirectToAction("Index", "Auth");
        }
    }
}