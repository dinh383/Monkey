using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monkey.Auth.Filters;
using Monkey.Auth.Interfaces;
using Monkey.Core.Models.Auth;
using Puppy.AutoMapper;
using System.Threading.Tasks;

namespace Monkey.Areas.Portal.Controllers
{
    [Route(Endpoint)]
    public class AuthController : MvcController
    {
        public const string Endpoint = AreaName + "/auth";
        public const string SignInEndpoint = "";
        public const string SignInSubmitEndpoint = "signin";
        public const string SignOutEndpoint = "signout";

        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [Route(SignInEndpoint)]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(new LoginModel());
        }

        [Route(SignInSubmitEndpoint)]
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

            // Sign In and get access token
            AccessTokenModel accessTokenModel = await _authenticationService.SignInAsync(requestToken).ConfigureAwait(true);

            // Sign In to Cookie, for web only
            await _authenticationService.SignInCookieAsync(Response.Cookies, accessTokenModel).ConfigureAwait(true);

            return RedirectToAction("Index", "Home");
        }

        [Route(SignOutEndpoint)]
        [HttpGet]
        [Auth]
        public async Task<IActionResult> SignOut()
        {
            await _authenticationService.SignOutCookieAsync(Response.Cookies).ConfigureAwait(true);

            return RedirectToAction("Index", "Auth");
        }
    }
}