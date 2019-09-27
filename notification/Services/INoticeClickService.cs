using System;
namespace notification.Services
{
    public enum NoticeClick
    {
        None, Defalut
    }

    public interface INoticeClickService
    {
        void Set(NoticeClick click);
        void Set(string click);
        NoticeClick Get();
    }
}