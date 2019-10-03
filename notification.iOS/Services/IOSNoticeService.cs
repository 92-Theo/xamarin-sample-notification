using System;
using notification.Services;

namespace notification.iOS.Services
{
    public class IOSNoticeService : INoticeService
    {
        public static readonly IOSNoticeService Instance = new IOSNoticeService();
        private NoticeInfo noticeInfo;
        private string deviceToken;

        public IOSNoticeService()
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
