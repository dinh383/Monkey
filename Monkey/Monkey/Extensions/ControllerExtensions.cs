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
            message = message?.Replace("'", "\'");

            controller.TempData.Set(Constants.TempDataKey.Notify, new NotifyResultViewModel
            {
                Title = title,
                Message = message,
                Status = status
            });
        }

        public static void RemoveNotify(this Controller controller)
        {
            controller.TempData.Remove(Constants.TempDataKey.Notify);
        }

        public enum NotifyStatus
        {
            [Display(Name = "success")]
            Success = 1,

            [Display(Name = "error")]
            Error = 2,

            [Display(Name = "warning")]
            Warning = 3,

            [Display(Name = "info")]
            Info = 4,
        }

        public class NotifyResultViewModel
        {
            public string Title { get; set; }
            public string Message { get; set; }
            public NotifyStatus Status { get; set; }
        }
    }
}