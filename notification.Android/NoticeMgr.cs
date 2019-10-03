using System;
using Android.App;
using Android.OS;
using Android.Content;
using Android.Support.V4.App;

using Android.Util;
using System.Collections.Generic;

namespace notification.Droid
{
    public class NoticeMgr
    {
        static readonly string TAG = "NoticeMgr";
        private global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity Parent { get; set; }

        private int Count { get; set; }


        public string ChannelId { get; private set; }
        public string ChannelName { get; private set; }
        public int NotifyId { get; private set; }
        public int IconId { get; set; }

        public bool IsHeadUp { get; set; }
        private int Priority { get { return (IsHeadUp ? NotificationCompat.PriorityHigh : NotificationCompat.PriorityLow); } }
        private NotificationImportance Importance { get { return (IsHeadUp ? NotificationImportance.High : NotificationImportance.Low); } }

        #region Constructor
        public NoticeMgr(global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity parent) : this(parent, "channelId_default", "channelName_default", 1)
        {
        }

        public NoticeMgr(global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity parent, string channelId, string channelName, int notifyId, bool bHeadUp = true)
        {
            Parent = parent;

            Count = 0;

            ChannelId = channelId;
            ChannelName = channelName;
            NotifyId = notifyId;
            IconId = 0;
            IsHeadUp = bHeadUp;


            CreateNotificationChannel();
        }
        #endregion

        public void Notify(string title, string body, string clickAction)
        {
            Notify(title, body, clickAction, null);
        }


        public void Notify(string title, string body, string clickAction, Dictionary<string, string> data)
        {
            Logger.CmWrite(TAG, $"{title}, {body}, {Count}");
            Count++;

            var intent = new Intent(clickAction)
                .AddFlags(ActivityFlags.SingleTop);
            if (data != null)
            {
                foreach(string key in data.Keys)
                {
                    intent.PutExtra(key, data[key]);
                }
            }

            var pendingIntent = PendingIntent.GetActivity(Parent,
                                                          NotifyId,
                                                          intent,
                                                          PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(Parent, ChannelId)
                                        .SetAutoCancel(true)
                                        .SetContentIntent(pendingIntent)
                                        .SetContentTitle(title)
                                        .SetPriority(Priority)
                                        .SetContentText(body);

            if (IconId != 0) notificationBuilder.SetSmallIcon(IconId);


            var notificationManager = NotificationManagerCompat.From(Parent);
            notificationManager.Notify(NotifyId, notificationBuilder.Build());
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                Logger.CmWrite(TAG, "ACK CreateNotificationChannel NO Surpport");
                return;
            }

            var channel = new NotificationChannel(ChannelId,
                                                  ChannelName,
                                                  Importance)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)(Parent.GetSystemService(Context.NotificationService));
            notificationManager.CreateNotificationChannel(channel);

            Logger.CmWrite(TAG, $"CreateNotificationChannel, {ChannelId}, {ChannelName}");
        }
    }
}
