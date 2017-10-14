using Monkey.Auth.Filters.Attributes;
using Monkey.Core;
using Monkey.Core.Exceptions;
using Monkey.Core.Models;
using Monkey.Core.Models.User;
using Monkey.Extensions;
using Monkey.Service.Auth;
using Monkey.Service.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Puppy.AutoMapper;
using Puppy.DataTable;
using Puppy.DataTable.Models.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enums = Monkey.Core.Constants.Enums;

namespace Monkey.Areas.Portal.Controllers
{
    [Route(Endpoint)]
    [Auth(Enums.Permission.Admin)]
    public class UserController : MvcController
    {
        public const string Endpoint = AreaName + "/user";
        public const string ListingEndpoint = "";
        public const string AddEndpoint = "add";
        public const string EditEndpoint = "{id}/edit";
        public const string SubmitEditEndpoint = "edit";
        public const string UpdateProfileEndpoint = "update-profile";
        public const string SubmitUpdateProfileEndpoint = "submit-update-profile";
        public const string CheckUniqueUserNameEndpoint = "check-unique-username";
        public const string CheckUniqueEmailEndpoint = "check-unique-email";
        public const string CheckExistEmailEndpoint = "check-exist-email";
        public const string CheckUniquePhoneEndpoint = "check-unique-phone";
        public const string RemoveEndpoint = "{id}/remove";

        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        #region Listing

        [Route(ListingEndpoint)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route(ListingEndpoint)]
        [HttpPost]
        public DataTableActionResult<UserModel> GetDataTable([FromForm] DataTableParamModel model)
        {
            var result = _userService.GetDataTableAsync(model, this.GetRequestCancellationToken());
            var response = result.Result.GetDataTableActionResult<UserModel>();
            return response;
        }

        #endregion Listing

        #region Add

        [Route(AddEndpoint)]
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.RoleSelectList = GetRoleSelectList();
            return View(new UserCreateModel());
        }

        [Route(AddEndpoint)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAdd([FromForm]UserCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.RoleSelectList = GetRoleSelectList();
                return View("Add", model);
            }

            await _userService.CreateByEmailAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);
            this.SetNotify("Add Success", "Add user successful", ControllerExtensions.NotifyStatus.Success);

            return RedirectToAction("Index");
        }

        #endregion Add

        #region Edit

        [Route(EditEndpoint)]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userModel = await _userService.GetAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);
            var userUpdateModel = userModel.MapTo<UserUpdateModel>();
            ViewBag.RoleSelectList = GetRoleSelectList();
            return View(userUpdateModel);
        }

        [Route(SubmitEditEndpoint)]
        [HttpPost]
        public async Task<IActionResult> SubmitEdit([FromForm]UserUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.RoleSelectList = GetRoleSelectList();
                return View("Edit", model);
            }

            await _userService.UpdateAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);
            this.SetNotify("Edit Success", "Edit user successful", ControllerExtensions.NotifyStatus.Success);
            return RedirectToAction("Index");
        }

        [Route(UpdateProfileEndpoint)]
        [HttpGet]
        public IActionResult UpdateProfile()
        {
            var updateProfileModel = LoggedInUser.Current.MapTo<UpdateProfileModel>();
            return View(updateProfileModel);
        }

        [Route(SubmitUpdateProfileEndpoint)]
        [HttpPost]
        public async Task<IActionResult> SubmitUpdateProfile([FromForm]UpdateProfileModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("UpdateProfile", model);
            }

            await _userService.UpdateProfileAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            this.SetNotify("Edit Profile Success", "Your profile updated new information", ControllerExtensions.NotifyStatus.Success);

            return RedirectToAction("Index", "Home");
        }

        #endregion Edit

        #region Remove

        [Route(RemoveEndpoint)]
        [HttpPost]
        public async Task<JsonResult> Remove(int id)
        {
            await _userService.RemoveAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);
            return Json(new { });
        }

        #endregion Remove

        #region Check

        [Route(CheckUniqueUserNameEndpoint)]
        [HttpPost]
        public JsonResult CheckUniqueUserName(string userName, int? id = null)
        {
            try
            {
                _userService.CheckUniqueUserName(userName, id);
                return Json(true);
            }
            catch (MonkeyException monkeyException)
            {
                if (monkeyException.Code == ErrorCode.UserNameNotUnique)
                {
                    return Json(false);
                }

                throw;
            }
        }

        [Route(CheckUniqueEmailEndpoint)]
        [HttpPost]
        public JsonResult CheckUniqueEmail(string email, int? id = null)
        {
            try
            {
                _userService.CheckUniqueEmail(email, id);
                return Json(true);
            }
            catch (MonkeyException monkeyException)
            {
                if (monkeyException.Code == ErrorCode.UserEmailNotUnique)
                {
                    return Json(false);
                }

                throw;
            }
        }

        [Route(CheckExistEmailEndpoint)]
        [HttpPost]
        public JsonResult CheckExistEmail(string email)
        {
            try
            {
                _userService.CheckExistEmail(email);
                return Json(true);
            }
            catch (MonkeyException monkeyException)
            {
                if (monkeyException.Code == ErrorCode.UserEmailNotExist)
                {
                    return Json(false);
                }

                throw;
            }
        }

        [Route(CheckUniquePhoneEndpoint)]
        [HttpPost]
        [AllowAnonymous]
        public JsonResult CheckUniquePhone(string phone, int? id = null)
        {
            try
            {
                _userService.CheckUniquePhone(phone, id);
                return Json(true);
            }
            catch (MonkeyException monkeyException)
            {
                if (monkeyException.Code == ErrorCode.UserPhoneNotUnique)
                {
                    return Json(false);
                }

                throw;
            }
        }

        #endregion Check

        #region Private Helper

        private List<SelectListItem> GetRoleSelectList()
        {
            return _roleService.GetListRoleAsync(new PagedCollectionParametersModel()).Result.Items.Select(x =>
                new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();
        }

        #endregion Private Helper
    }
}