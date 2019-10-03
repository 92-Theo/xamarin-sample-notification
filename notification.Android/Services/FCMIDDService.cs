using System;
using Android.App;
using Firebase.Iid;
using Android.Util;

namespace notification.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FCMIDDService : FirebaseInstanceIdService
    {
        const string TAG = "FCMIDDService";

        public FCMIDDService()
        {
        }

        public override void OnTokenRefresh()
        {
            var newToken = Common.GetDeviceToken();
            AndroidNoticeService.Instance.SetDeviceToken(newToken);
            Logger.CmWrite(TAG, "Device token: " + newToken);
            MainActivity.CmRefreshToken();
        }
    }
}
