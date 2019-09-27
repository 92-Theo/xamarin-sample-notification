using System;
using notification.iOS;
using Xamarin.Forms;

using notification.Services;

[assembly: Dependency(typeof(IOSNoticeClickServiceWrapper))]
namespace notification.iOS
{
    public class IOSNoticeClickServiceWrapper : INoticeClickService
    {
        private readonly INoticeClickService noticeClickService;

        public IOSNoticeClickServiceWrapper()
        {
            noticeClickService = IOSNoticeClickService.Instance;
        }

        public NoticeClick Get()
        {
            return this.noticeClickService.Get();
        }

        public void Set(NoticeClick click)
        {
            throw new NotSupportedException();
        }

        public void Set(string click)
        {
            throw new NotSupportedException();
        }
    }
}
