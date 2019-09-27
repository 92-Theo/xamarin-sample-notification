using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using notification.Droid.Notification;
using notification.Droid.Common;
using Firebase.Iid;

namespace notification.Droid
{
    [Activity(Label = "notification", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { "page1", "page2" }, Categories = new[] { "android.intent.category.DEFAULT" })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        private App app;
        private NotificationMgr notifyMgr;
        private Log log;

        
        #region Override
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;
            log = new Log("MainActivity");
            log.Write($"OnCreate: Action: {Intent.Action}");
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            notifyMgr = new NotificationMgr(this);
            notifyMgr.IconId = Resource.Drawable.ic_stat_freejob_notification;
            if (!FCMClient.Common.IsGoogleApiAvailability(this))
            {
                log.Write("NAK IsGoogleApiAvailability");
            }

            app = new App();
            app.AddLocalNotify(NotifyLocal);
            app.AddTokenGet(GetToken);
            LoadApplication(app);
        }
        protected override void OnStart()
        {
            base.OnStart();
            // string txt = $"{AlarmActivity.CurDebugStr}\n{(AlarmActivity.IsAlarm?"Alarm":"NO Alarm")}";
            // app.SetTxtShow(txt);

            log.Write("OnStart");
        }
        protected override void OnResume()
        {
            base.OnResume();
            
            log.Write("OnResume");
        }
        protected override void OnStop()
        {
            base.OnStop();

            log.Write("OnStop");
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

            log.Write("OnDestroy");
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        #endregion


        #region Event Func
        void NotifyLocal()
        {
            log.Write("CALL NotifyLocal");
            notifyMgr.Notify("제목이라우", "내용인디", "MAIN");
        }

        void GetToken()
        {
            log.Write("token: " + FirebaseInstanceId.Instance.Token);
        }
        #endregion

        public void Notify (string title, string body, string clickAction)
        {
            notifyMgr.Notify(title, body, clickAction);
        }
    }
}