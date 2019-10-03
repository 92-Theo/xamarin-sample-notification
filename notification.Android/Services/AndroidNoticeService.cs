using System;
using Firebase.Iid;
using notification.Services;
namespace notification.Droid.Services
{
    public class AndroidNoticeService : INoticeService
    {
        public static readonly AndroidNoticeService Instance = new AndroidNoticeService();

        private NoticeInfo noticeInfo;
        private string deviceToken;

        public AndroidNoticeService()
        {
            noticeInfo.Empty();
            deviceToken = default(string);
        }

        #region Info
        public void SetInfo(NoticeInfo info)
        {
            this.noticeInfo = info.Copy();
        }

        public NoticeInfo GetInfo()
        {
            NoticeInfo tmp = noticeInfo.Copy();
            noticeInfo.Empty();

            return tmp;
        }
        #endregion


        #region Device Token
        public string GetDeviceToken()
        {
            return deviceToken;
        }

        public void SetDeviceToken(string token)
        {
            deviceToken = token;
        }
        #endregion
    }
}
