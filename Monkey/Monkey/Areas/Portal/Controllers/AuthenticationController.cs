using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monkey.Authentication;
using Monkey.Core.Models.User;
using Monkey.Service;
using Puppy.AutoMapper;
using System.Threading.Tasks;

namespace Monkey.Areas.Portal.Controllers
{
    [Route(AreaName + "/auth")]
    public class AuthenticationController : MvcController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
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
            requestToken.ClientId = AuthenticationConfig.SystemClientId;
            requestToken.ClientSecret = AuthenticationConfig.SystemClientSecret;

            AccessTokenModel accessToken = await _authenticationService.GetTokenAsync(requestToken).ConfigureAwait(true);

            TokenHelper.SetAccessTokenToCookie(Response.Cookies, accessToken);

            return View("Index", model);
        }
    }
}