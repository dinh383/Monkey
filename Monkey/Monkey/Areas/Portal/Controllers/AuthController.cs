using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monkey.Auth.Filters;
using Monkey.Auth.Interfaces;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.Auth;
using Monkey.Extensions;
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
        public IActionResult Index(string redirectUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginModel(redirectUrl));
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

            try
            {
                // Sign In and get access token
                AccessTokenModel accessTokenModel = await _authenticationService.SignInAsync(requestToken).ConfigureAwait(true);

                // Sign In to Cookie, for web only
                await _authenticationService.SignInCookieAsync(Response.Cookies, accessTokenModel).ConfigureAwait(true);

                if (string.IsNullOrWhiteSpace(model.RedirectUrl))
                {
                    return RedirectToAction("Index", "Home");
                }

                return Redirect(model.RedirectUrl);
            }
            catch (MonkeyException ex)
            {
                if (ex.Code == ErrorCode.UserInActive)
                {
                    this.SetNotify("SignIn Fail", "Your account is in-active, please active via your email and try sign-in again", ControllerExtensions.NotifyStatus.Error);
                    return View("Index", model);
                }

                if (ex.Code == ErrorCode.UserBanned)
                {
                    this.SetNotify("SignIn Fail", $"Your account is banned! {ex.Message}", ControllerExtensions.NotifyStatus.Error);
                    return View("Index", model);
                }

                if (ex.Code == ErrorCode.UserNameNotExist)
                {
                    this.SetNotify("SignIn Fail", "Your account is not exist", ControllerExtensions.NotifyStatus.Error);
                    return View("Index", model);
                }

                if (ex.Code == ErrorCode.UserPasswordWrong)
                {
                    this.SetNotify("SignIn Fail", "Your password is wrong, please try again", ControllerExtensions.NotifyStatus.Error);
                    return View("Index", model);
                }

                throw;
            }
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