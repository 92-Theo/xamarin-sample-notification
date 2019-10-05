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
            Logger.CmWrite(TAG, "OnCreate");
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

        protected override void OnStart()
        {
            Logger.CmWrite(TAG, "OnStart");
            base.OnStart();
        }

        protected override void OnNewIntent(Intent intent)
        {
            Logger.CmWrite(TAG, "OnNewIntent");

            base.OnNewIntent(intent);
            Intent.SetAction(intent.Action);
            if (intent.Extras != null)
                Intent.PutExtras(intent.Extras);
        }

        protected override void OnResume()
        {
            Logger.CmWrite(TAG, "OnResume");
            base.OnResume();
            
            
            if (FindNoticeInfo(Intent))
            {
                CmRequestNoticePage();
            }
        }

        protected override void OnPause()
        {
            Logger.CmWrite(TAG, "OnPause");
            base.OnPause();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        #endregion


        private bool FindNoticeInfo (Intent intent)
        {
            Logger.CmWrite(TAG, "FindNoticeInfo");
            Bundle bundle = intent.Extras;
            bool bRet = false;
            if (bundle != null)
            {
                object myvalue = bundle.Get("mykey");
                if (myvalue != null)
                {
                    NoticeInfo info;
                    info.action = intent.Action;
                    info.data = myvalue.ToString();
                    
                    AndroidNoticeService.Instance.SetInfo(info);
                    bRet = true;
                }
                // Init
                intent.SetAction("");
                intent.RemoveExtra("mykey");
                ShowNoticeInfo(intent);
            }
            return bRet;
        }

        private void ShowNoticeInfo (Intent intent)
        {
            if (intent == null) return;

            Logger.CmWrite(TAG, $"action: {intent.Action}");
            Bundle bundle = intent.Extras;
            if (bundle == null) return;

            ICollection<string> keys = bundle.KeySet();
            foreach (string key in keys)
            {
                object value = bundle.Get(key);
                if (value == null) continue;

                Logger.CmWrite(TAG, $"key: {key}, value: {value.ToString()}");
            }
        }
        
        void NotifyLocal()
        {
            Logger.CmWrite(TAG,"CALL NotifyLocal");
            CmNotify("제목이라우", "내용인디", "MAIN", null);
        }

        void GetToken()
        {
            Logger.CmWrite(TAG, "GetToken");
            Logger.CmWrite(TAG, $"token: {Services.Common.GetDeviceToken()}");
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
