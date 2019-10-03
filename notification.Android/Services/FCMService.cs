using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Messaging;

namespace notification.Droid.Services
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

            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (string key in message.Data.Keys)
            {
                string value = message.Data[key];
                Logger.CmWrite(TAG, $"key: {key}, value:{value}");
                data.Add(key, value);
            }

            Logger.CmWrite(TAG, $"OnMessageReceived: {title}, {body}, {clickAction}");
            MainActivity.CmNotify(title, body, clickAction, data);
        }
    }
}
