using Microsoft.AspNetCore.Mvc;
using Monkey.Auth.Filters;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.Auth;
using Monkey.Service;
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
        public const string EditEndpoint = "edit/{id}";
        public const string SubmitEditEndpoint = "edit";
        public const string CheckUniqueNameEndpoint = "check-unique-name";

        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

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
            var result = _clientService.GetDataTableAsync(model);
            var response = result.Result.GetDataTableActionResult<ClientModel>();
            return response;
        }

        [Route(AddEndpoint)]
        [HttpGet]
        public IActionResult Add()
        {
            return View(new ClientCreateModel());
        }

        [Route(AddEndpoint)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAdd([FromForm]ClientCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", model);
            }

            await _clientService.CreateAsync(model).ConfigureAwait(true);

            return View("Index");
        }

        [Route(EditEndpoint)]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var clientModel = await _clientService.GetAsync(id).ConfigureAwait(true);
            var clientUpdateModel = clientModel.MapTo<ClientUpdateModel>();
            return View(clientUpdateModel);
        }

        [Route(SubmitEditEndpoint)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitEdit([FromForm]ClientUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            await _clientService.UpdateAsync(model).ConfigureAwait(true);

            return View("Index");
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
                if (monkeyException.Code == ErrorCode.ClientNameAlreadyExist)
                {
                    return Json(false);
                }

                throw;
            }
        }
    }
}