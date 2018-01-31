using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monkey.Auth.Helpers;
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

        // SignIn and SignOut
        public const string SignInEndpoint = "";

        public const string SignInSubmitEndpoint = "signin";
        public const string SignOutEndpoint = "signout";

        // Confirm Email
        public const string ConfirmEmailEndpoint = "confirm-email";

        public const string SubmitConfirmEmailEndpoint = "submit-email-confirm";

        // Set Password
        public const string SetPasswordEndpoint = "set-password";

        public const string SubmitSetPasswordEndpoint = "submit-set-password";

        // Cannot SignIn
        public const string CannotSigInEndpoint = "cannot-signin";

        public const string SubmitCannotSigInEndpoint = "submit-cannot-signin";

        // Change Password
        public const string ChangePasswordEndpoint = "change-password";

        public const string SubmitChangePasswordEndpoint = "submit-change-password";
        public const string CheckCurrentPasswordEndpoint = "check-current-password";

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

            return View(new SignInModel(redirectUrl));
        }

        #region SignIn and SignOut

        [Route(SignInSubmitEndpoint)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            RequestTokenModel requestToken = model.MapTo<RequestTokenModel>();

            try
            {
                // Sign In and get access token
                AccessTokenModel accessTokenModel = await _authenticationService.SignInAsync(HttpContext, requestToken, this.GetRequestCancellationToken()).ConfigureAwait(true);

                // Sign In to Cookie, for web only
                await _authenticationService.SignInCookieAsync(HttpContext, accessTokenModel, this.GetRequestCancellationToken()).ConfigureAwait(true);

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
                    this.SetNotify("Sign-in Fail", "Your account is in-active, please active via your email and try sign-in again", NotifyStatus.Error);
                    return View("Index", model);
                }

                if (ex.Code == ErrorCode.UserBanned)
                {
                    this.SetNotify("Sign-in Fail", $"Your account is banned! {ex.Message}", NotifyStatus.Error);
                    return View("Index", model);
                }

                if (ex.Code == ErrorCode.NotFound)
                {
                    this.SetNotify("Sign-in Fail", "Your account is not exist", NotifyStatus.Error);
                    return View("Index", model);
                }

                if (ex.Code == ErrorCode.UserPasswordWrong)
                {
                    this.SetNotify("Sign-in Fail", "Your password is wrong, please try again", NotifyStatus.Error);
                    return View("Index", model);
                }

                throw;
            }
        }

        [Route(SignOutEndpoint)]
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _authenticationService.SignOutCookieAsync(HttpContext, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return RedirectToAction("Index", "Auth");
        }

        #endregion SignIn and SignOut

        #region Cannot SignIn

        [Route(CannotSigInEndpoint)]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult CannotSignIn()
        {
            return View(new CannotSigInModel());
        }

        [Route(SubmitCannotSigInEndpoint)]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SubmitCannotSignIn([FromForm] CannotSigInModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CannotSignIn", model);
            }

            await _authenticationService.SendConfirmEmailOrSetPasswordAsync(model.Email, this.GetRequestCancellationToken()).ConfigureAwait(true);

            this.SetNotify("Send Success", "Please check your email inbox to active or set new password", NotifyStatus.Success);

            return RedirectToAction("Index");
        }

        #endregion Cannot SignIn

        #region Confirm Email

        [Route(ConfirmEmailEndpoint)]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmEmail(string token)
        {
            bool isExpireOrInvalidToken = _authenticationService.IsExpireOrInvalidConfirmEmailToken(token);

            if (isExpireOrInvalidToken)
            {
                this.SetNotify("Confirm Email Fail", "Your link is invalid or expired", NotifyStatus.Error);

                return RedirectToAction("CannotSignIn");
            }

            var activeModel = new SetPasswordModel
            {
                Token = token
            };

            return View(activeModel);
        }

        [Route(SubmitConfirmEmailEndpoint)]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SubmitConfirmEmail([FromForm] SetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmEmail", model);
            }
            try
            {
                await _authenticationService.ConfirmEmailAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

                this.SetNotify("Active Success", "Now you can sign-in to the system", NotifyStatus.Success);

                return RedirectToAction("Index");
            }
            catch (MonkeyException ex)
            {
                if (ex.Code == ErrorCode.UserConfirmEmailTokenExpireOrInvalid)
                {
                    this.SetNotify("Confirm Email Fail", "Your link is invalid or expired", NotifyStatus.Error);
                    return RedirectToAction("CannotSignIn");
                }
                throw;
            }
        }

        #endregion Confirm Email

        #region Set Password

        [Route(SetPasswordEndpoint)]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SetPassword(string token)
        {
            bool isExpireOrInvalidToken = _authenticationService.IsExpireOrInvalidSetPasswordToken(token);

            if (isExpireOrInvalidToken)
            {
                this.SetNotify("Set Password Fail", "Your link is invalid or expired", NotifyStatus.Error);

                return RedirectToAction("CannotSignIn");
            }

            var activeModel = new SetPasswordModel
            {
                Token = token
            };

            return View(activeModel);
        }

        [Route(SubmitSetPasswordEndpoint)]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SubmitSetPassword([FromForm] SetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SetPassword", model);
            }

            try
            {
                await _authenticationService.SetPasswordAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

                this.SetNotify("Set Password Success", "Now you can sign-in to the system", NotifyStatus.Success);

                return RedirectToAction("Index");
            }
            catch (MonkeyException ex)
            {
                if (ex.Code == ErrorCode.UserSetPasswordTokenExpireOrInvalid)
                {
                    this.SetNotify("Set Password Fail", "Your link is invalid or expired", NotifyStatus.Error);
                    return RedirectToAction("CannotSignIn");
                }
                throw;
            }
        }

        #endregion Set Password

        #region Change Password

        [Route(ChangePasswordEndpoint)]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordModel());
        }

        [Route(SubmitChangePasswordEndpoint)]
        [HttpPost]
        public async Task<IActionResult> SubmitChangePassword([FromForm] ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ChangePassword", model);
            }

            try
            {
                await _authenticationService.ChangePasswordAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

                this.SetNotify("Change Password Success", "Now you can sign-in with new password", NotifyStatus.Success);

                return RedirectToAction("Index");
            }
            catch (MonkeyException ex)
            {
                if (ex.Code == ErrorCode.UserPasswordWrong)
                {
                    this.SetNotify("Change Password Fail", "Current password is wrong", NotifyStatus.Error);
                    return RedirectToAction("ChangePassword");
                }
                throw;
            }
        }

        [Route(CheckCurrentPasswordEndpoint)]
        [HttpPost]
        public JsonResult CheckCurrentPassword(string currentPassword)
        {
            try
            {
                _authenticationService.CheckCurrentPassword(currentPassword);

                return Json(true);
            }
            catch (MonkeyException monkeyException)
            {
                if (monkeyException.Code == ErrorCode.UserPasswordWrong)
                {
                    return Json(false);
                }

                throw;
            }
        }

        #endregion Change Password
    }
}