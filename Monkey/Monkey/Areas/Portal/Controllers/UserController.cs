using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Monkey.Auth.Filters;
using Monkey.Core.Exceptions;
using Monkey.Core.Models;
using Monkey.Core.Models.Auth;
using Monkey.Extensions;
using Monkey.Service;
using Puppy.AutoMapper;
using Puppy.DataTable;
using Puppy.DataTable.Models.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Monkey.Service.Auth;
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
            var result = _userService.GetDataTableAsync(model);
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

            await _userService.CreateByEmailAsync(model).ConfigureAwait(true);
            this.SetNotify("Add Success", "Add user successful", ControllerExtensions.NotifyStatus.Success);

            return RedirectToAction("Index");
        }

        #endregion Add

        #region Edit

        [Route(EditEndpoint)]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userModel = await _userService.GetAsync(id).ConfigureAwait(true);
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

            await _userService.UpdateAsync(model).ConfigureAwait(true);
            this.SetNotify("Edit Success", "Edit user successful", ControllerExtensions.NotifyStatus.Success);
            return RedirectToAction("Index");
        }

        #endregion Edit

        [Route(RemoveEndpoint)]
        [HttpPost]
        public async Task<JsonResult> Remove(int id)
        {
            await _userService.RemoveAsync(id).ConfigureAwait(true);
            return Json(new { });
        }

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

        private List<SelectListItem> GetRoleSelectList()
        {
            return _roleService.GetListRoleAsync(new PagedCollectionParametersModel()).Result.Items.Select(x =>
                new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();
        }
    }
}