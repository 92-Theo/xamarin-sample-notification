using System;
using notification.Services;
namespace notification.iOS
{
    public class IOSNoticeClickService : INoticeClickService
    {
        public static readonly IOSNoticeClickService Instance = new IOSNoticeClickService();
        private NoticeClick value;


        public NoticeClick Get()
        {
            return value;
        }

        public void Set(NoticeClick click)
        {
            value = click;   
        }

        public void Set(string click)
        {
            if (click == default(string))
                value = NoticeClick.None;
            else
                value = NoticeClick.Defalut;
        }

    }
}
