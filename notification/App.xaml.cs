using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using notification.Services;

namespace notification
{
    public partial class App : Application
    {
        private static readonly string TAG = "App";
        public DefaultEventHandler AppOnForeground;
        public DefaultEventHandler AppOnBackground;

        public App()
        {
            IsInitNotice = false;
            IsInitToken = false;
            InitializeComponent();
            
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            OnForeground();

            IsInitNotice = true;
            IsInitToken = true;

            RequestNoticePage();
            RefreshToken();
        }

        protected override void OnSleep()
        {
            OnBackground();
        }

        protected override void OnResume()
        {
            OnForeground();
        }

        private void OnForeground()
        {
            AppOnForeground?.Invoke();
        }


        private void OnBackground()
        {
            AppOnBackground?.Invoke();
        }

        #region EventHandler with native app
        public void AddLocalNotify(DefaultEventHandler eventHandler) => ((MainPage)MainPage).LocalNotify += eventHandler;
        public void AddTokenGet(DefaultEventHandler eventHandler) => ((MainPage)MainPage).TokenGet += eventHandler;
        public void SetTxtShow(string txt) => ((MainPage)MainPage).SetTxtShow(txt);
        #endregion
    }
}
