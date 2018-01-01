using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monkey.Auth.Filters.Attributes;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.Auth;
using Monkey.Extensions;
using Monkey.Service.Auth;
using Puppy.AutoMapper;
using Puppy.DataTable;
using Puppy.DataTable.Models.Request;
using System.Threading.Tasks;
using Enums = Monkey.Core.Constants.Enums;

namespace Monkey.Areas.Portal.Controllers
{
    [Route(Endpoint)]
    [Auth(Enums.Permission.Admin)]
    public class ClientController : MvcController
    {
        public const string Endpoint = AreaName + "/client";
        public const string ListingEndpoint = "";
        public const string AddEndpoint = "add";
        public const string EditEndpoint = "{id}/edit";
        public const string SubmitEditEndpoint = "edit";
        public const string CheckUniqueNameEndpoint = "check-unique-name";
        public const string RemoveEndpoint = "{id}/remove";
        public const string GenerateSecretEndpoint = "{id}/generate-secret";

        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
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
        public DataTableActionResult<ClientModel> GetDataTable([FromForm] DataTableParamModel model)
        {
            var result = _clientService.GetDataTableAsync(model, this.GetRequestCancellationToken());
            var response = result.Result.GetDataTableActionResult<ClientModel>();
            return response;
        }

        #endregion Listing

        #region Add

        [Route(AddEndpoint)]
        [HttpGet]
        public IActionResult Add()
        {
            return View(new CreateClientModel());
        }

        [Route(AddEndpoint)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAdd([FromForm]CreateClientModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", model);
            }

            await _clientService.CreateAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);
            this.SetNotify("Add Success", "Add client successful", NotifyStatus.Success);

            return RedirectToAction("Index");
        }

        #endregion Add

        #region Edit

        [Route(EditEndpoint)]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var clientModel = await _clientService.GetAsync(id).ConfigureAwait(true);
            var clientUpdateModel = clientModel.MapTo<UpdateClientModel>();
            return View(clientUpdateModel);
        }

        [Route(SubmitEditEndpoint)]
        [HttpPost]
        public async Task<IActionResult> SubmitEdit([FromForm]UpdateClientModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            await _clientService.UpdateAsync(model).ConfigureAwait(true);
            this.SetNotify("Edit Success", "Edit client successful", NotifyStatus.Success);

            return RedirectToAction("Index");
        }

        #endregion Edit

        [Route(RemoveEndpoint)]
        [HttpPost]
        public async Task<JsonResult> Remove(int id)
        {
            await _clientService.RemoveAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);
            return Json(new { });
        }

        [Route(GenerateSecretEndpoint)]
        [HttpPost]
        public async Task<JsonResult> GenerateSecret(int id)
        {
            try
            {
                string secret = await _clientService.GenerateSecretAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);
                return Json(new
                {
                    secret
                });
            }
            catch (MonkeyException ex)
            {
                ErrorModel errorModel = new ErrorModel(ex.Code, ex.Message, ex.AdditionalData);
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Json(errorModel);
            }
        }

        [Route(CheckUniqueNameEndpoint)]
        [HttpPost]
        public JsonResult CheckUniqueName(string name, int? id = null)
        {
            try
            {
                _clientService.CheckUniqueName(name, id);
                return Json(true);
            }
            catch (MonkeyException monkeyException)
            {
                if (monkeyException.Code == ErrorCode.ClientNameNotUnique)
                {
                    return Json(false);
                }

                throw;
            }
        }
    }
}