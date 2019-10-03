using System;
using Xamarin.Forms;

using notification.Services;


namespace notification
{
    public partial class App
    {
        public bool IsInitNotice { get; private set; }
        public bool IsInitToken { get; private set; }

        public void RequestNoticePage()
        {
            if (IsInitNotice == false)
                return;

            NoticeInfo info = DependencyService.Get<INoticeService>().GetInfo();
            //
            // process ... by info
            //
        }

        public void RefreshToken()
        {
            if (IsInitToken == false)
                return;

            string token = DependencyService.Get<INoticeService>().GetDeviceToken();
            //
            // process ... by token
            //
        }
    }
}
