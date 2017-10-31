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
        private string _message;

        public string Title { get; set; }

        public string Message
        {
            get => _message;
            set
            {
                _message = _message?.Replace(@"'", @"\'")?.Replace(@"""", @"\""");
                _message = value;
            }
        }

        public NotifyStatus Status { get; set; }

        public NotifyResultViewModel()
        {
        }

        public NotifyResultViewModel(string title, string message, NotifyStatus status)
        {
            Title = title;
            Message = message;
            Status = status;
        }
    }
}