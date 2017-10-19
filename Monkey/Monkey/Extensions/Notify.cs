using System.ComponentModel.DataAnnotations;

namespace Monkey.Extensions
{
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