using System;
using System.Collections.Generic;

namespace notification.Services
{
    public struct NoticeInfo
    {
        public string action;
        public string data;

        public NoticeInfo Copy()
        {
            NoticeInfo tmp;
            tmp.action = action;
            tmp.data = data;

            return tmp;
        }

        public void Empty()
        {
            action = default(string);
            data = default(string);
        }
    }

    public interface INoticeService
    {
        void SetInfo(NoticeInfo info);
        NoticeInfo GetInfo();
        
        void SetDeviceToken(string token);
        string GetDeviceToken();
    }
}