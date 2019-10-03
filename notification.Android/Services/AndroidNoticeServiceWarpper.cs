using System;
using notification.Services;
using notification.Droid.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidNoticeServiceWarpper))]
namespace notification.Droid.Services
{
    public class AndroidNoticeServiceWarpper : INoticeService
    {
        private readonly INoticeService noticeService;

        public AndroidNoticeServiceWarpper()
        {
            noticeService = AndroidNoticeService.Instance;
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
