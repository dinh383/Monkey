using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monkey.Auth.Filters.Attributes;
using Monkey.Core.Exceptions;
using Monkey.Extensions;
using Puppy.AutoMapper;
using Puppy.DataTable;
using Puppy.DataTable.Models.Request;
using System.Threading.Tasks;
using Monkey.Core.Models;
using Monkey.Service;
using Enums = Monkey.Core.Constants.Enums;

namespace Monkey.Areas.Portal.Controllers
{
    [Route(Endpoint)]
    [Auth(Enums.Permission.Admin)]
    public class ImageController : MvcController
    {
        public const string Endpoint = AreaName + "/image";
        public const string ListingEndpoint = "";
        public const string AddEndpoint = "add";
        public const string EditEndpoint = "{id}/edit";
        public const string SubmitEditEndpoint = "edit";
        public const string RemoveEndpoint = "{id}/remove";

        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
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
        public DataTableActionResult<ImageModel> GetDataTable([FromForm] DataTableParamModel model)
        {
            var result = _imageService.GetDataTableAsync(model, this.GetRequestCancellationToken());
            var response = result.Result.GetDataTableActionResult<ImageModel>();
            return response;
        }

        #endregion Listing

        #region Add

        [Route(AddEndpoint)]
        [HttpGet]
        public IActionResult Add()
        {
            return View(new AddImageModel());
        }

        [Route(AddEndpoint)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAdd([FromForm]AddImageModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", model);
            }

            await _imageService.CreateAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);
            this.SetNotify("Add Success", "Add Image successful", NotifyStatus.Success);

            return RedirectToAction("Index");
        }

        #endregion Add

        #region Edit

        [Route(EditEndpoint)]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var imageModel = await _imageService.GetAsync(id).ConfigureAwait(true);
            var imageUpdateModel = imageModel.MapTo<UpdateImageModel>();
            return View(imageUpdateModel);
        }

        [Route(SubmitEditEndpoint)]
        [HttpPost]
        public async Task<IActionResult> SubmitEdit([FromForm]UpdateImageModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            await _imageService.UpdateAsync(model).ConfigureAwait(true);
            this.SetNotify("Edit Success", "Edit Image successful", NotifyStatus.Success);

            return RedirectToAction("Index");
        }

        #endregion Edit

        [Route(RemoveEndpoint)]
        [HttpPost]
        public async Task<JsonResult> Remove(int id)
        {
            await _imageService.RemoveAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);
            return Json(new { });
        }
    }
}