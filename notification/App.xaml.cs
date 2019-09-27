using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using notification.Services;

namespace notification
{
    public partial class App : Application
    {
        public NoticeClick  CurNoticeClick { get; set; } // 현재 클릭 상태

        public EventHandler AppOnForeground;
        public EventHandler AppOnBackground;



        public App()
        {
            System.Console.WriteLine("Create App");
            InitializeComponent();
            CurNoticeClick = NoticeClick.None;
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            System.Console.WriteLine("OnStart");
            // Handle when your app starts
            OnForeground();
        }

        protected override void OnSleep()
        {
            System.Console.WriteLine("OnSleep");
            // Handle when your app sleeps
            OnBackground();
        }

        protected override void OnResume()
        {
            System.Console.WriteLine("OnResume");
            // Handle when your app resumes
            OnForeground();
        }

        private void OnForeground()
        {
            AppOnForeground?.Invoke();
            // CurNoticeClick = DependencyService.Get<INoticeClickService>().Get();

            // string value = (CurNoticeClick == NoticeClick.None ? "NONE" : "DEFALUT");
            // System.Console.WriteLine($"OnForeground: {value}");
            // SetTxtShow(value);
        }


        private void OnBackground()
        {
            AppOnBackground?.Invoke();
        }

        #region EventHandler with native app
        public void AddLocalNotify(EventHandler eventHandler) => ((MainPage)MainPage).LocalNotify += eventHandler;
        public void AddTokenGet(EventHandler eventHandler) => ((MainPage)MainPage).TokenGet += eventHandler;
        public void SetTxtShow(string txt) => ((MainPage)MainPage).SetTxtShow(txt);
        #endregion
    }
}
