using Microsoft.AspNetCore.Mvc;
using Puppy.Web;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace Monkey.Extensions
{
    public static class ControllerExtensions
    {
        public static CancellationToken GetRequestCancellationToken(this Controller controller)
        {
            return controller.HttpContext?.RequestAborted ?? CancellationToken.None;
        }

        public static void SetNotify(this Controller controller, string title, string message, NotifyStatus status = NotifyStatus.Info)
        {
            controller.TempData.Set(Constants.TempDataKey.Notify, new NotifyResultViewModel
            {
                Title = title,
                Message = message?.Replace("'", "\'"),
                Status = status
            });
        }

        public static void RemoveNotify(this Controller controller)
        {
            controller.TempData.Remove(Constants.TempDataKey.Notify);
        }
    }
}