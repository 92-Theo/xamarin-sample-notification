using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Util;
using Firebase.Messaging;

namespace notification.Droid.FCMClient
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FCMService : FirebaseMessagingService
    {
        const string TAG = "FCMService";

        public FCMService()
        {
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            var title = message.GetNotification().Title;
            var body = message.GetNotification().Body;
            var clickAction = message.GetNotification().ClickAction;

            Log.Debug(TAG, $"{title}, {body}");
            MainActivity.Instance?.Notify(title, body, clickAction);
        }
    }
}
