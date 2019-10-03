using System;
using notification.Services;
using notification.iOS.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(IOSNoticeServiceWarpper))]
namespace notification.iOS.Services
{
    public class IOSNoticeServiceWarpper : INoticeService
    {
        private readonly INoticeService noticeService;

        public IOSNoticeServiceWarpper()
        {
            noticeService = IOSNoticeService.Instance;
        }


        #region Info
        public void SetInfo(NoticeInfo info)
        {
            throw new NotSupportedException();
        }

        public NoticeInfo GetInfo()
        {
            return noticeService.GetInfo();
        }
        #endregion

        #region Device Token
        public string GetDeviceToken()
        {
            return noticeService.GetDeviceToken();
        }

        public void SetDeviceToken(string token)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
