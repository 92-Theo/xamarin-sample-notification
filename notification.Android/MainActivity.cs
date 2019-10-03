using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

using System.Collections.Generic;
using Android.Content;

using notification.Services;
using notification.Droid.Services;

namespace notification.Droid
{
    [Activity(Label = "notification", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { "MAIN" }, Categories = new[] { "android.intent.category.DEFAULT" })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        private static readonly string TAG = "Droid.MainActivity";
        private NoticeMgr noticeMgr;

        App mainApp;
        
        #region Override Func
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;

            #region Initialize
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            //
            // Notifice
            noticeMgr = new NoticeMgr(this);
            noticeMgr.IconId = Resource.Drawable.ic_stat_freejob_notification;
            if (!Services.Common.IsGoogleApiAvailability(this))
            {
                Logger.CmWrite("NAK IsGoogleApiAvailability");
            }
            #endregion

            #region Load App
            mainApp = new App();
            mainApp.AddLocalNotify(NotifyLocal);
            mainApp.AddTokenGet(GetToken);
            LoadApplication(mainApp);
            #endregion
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            Intent.SetAction(intent.Action);
            Intent.PutExtras(intent.Extras);
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (FindNoticeInfo(Intent))
            {
                CmRequestNoticePage();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        #endregion


        private bool FindNoticeInfo (Intent intent)
        {
            Bundle bundle = Intent.Extras;
            if (bundle != null)
            {
                object myvalue = bundle.Get("mykey");
                if (myvalue != null)
                {
                    NoticeInfo info;
                    info.action = intent.Action;
                    info.data = myvalue.ToString();

                    AndroidNoticeService.Instance.SetInfo(info);
                    return true;
                }
            }
            return false;
        }
        
        void NotifyLocal()
        {
            Logger.CmWrite(TAG,"CALL NotifyLocal");
            CmNotify("제목이라우", "내용인디", "MAIN", null);
        }

        void GetToken()
        {
            mainApp.SetTxtShow($"token: {Services.Common.GetDeviceToken()}");
        }



        public static void CmNotify(string title, string body, string clickAction, Dictionary<string, string> data)
        {
            Instance?.noticeMgr.Notify(title, body, clickAction, data);
        }

        public static void CmRequestNoticePage()
        {
            Instance?.mainApp.RequestNoticePage();
        }

        public static void CmRefreshToken()
        {
            Instance?.mainApp.RefreshToken();
        }
    }
}
