using System;
using Android.App;
using Firebase.Iid;
using Android.Util;

namespace notification.Droid.FCMClient
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
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            SendRegistrationToServer(refreshedToken);
        }
        void SendRegistrationToServer(string token)
        {
            // Add custom implementation, as needed.
        }
    }
}
