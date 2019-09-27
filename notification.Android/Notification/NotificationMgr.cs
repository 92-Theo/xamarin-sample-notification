using System;
using Android.App;
using Android.OS;
using Android.Content;
using Android.Support.V4.App;

using notification.Droid.Common;


namespace notification.Droid.Notification
{
    public class NotificationMgr
    {
        private global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity Parent { get; set; }

        private Log log;
        private int Count { get; set; }


        public string ChannelId { get; private set; }
        public string ChannelName { get; private set; }
        public int NotifyId { get; private set; }
        public int IconId { get; set; }

        public bool IsHeadUp { get; set; }
        private int Priority { get { return (IsHeadUp ? NotificationCompat.PriorityHigh : NotificationCompat.PriorityLow); } }
        private NotificationImportance Importance { get { return (IsHeadUp ? NotificationImportance.High : NotificationImportance.Low); } }

        #region Constructor
        public NotificationMgr(global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity parent) : this(parent, "channelId_default", "channelName_default", 1)
        {
        }

        public NotificationMgr(global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity parent, string channelId, string channelName, int notifyId, bool bHeadUp = true)
        {
            Parent = parent;

            log = new Log("NotifiyManager");


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
            Count++;

            log.Write($"{title}, {body}, {Count}");

            var intent = new Intent(clickAction)
                .AddFlags(ActivityFlags.ClearTop);


            var pendingIntent = PendingIntent.GetActivity(Parent,
                                                          NotifyId,
                                                          intent,
                                                          PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(Parent, ChannelId)
                                        .SetAutoCancel(true)
                                        .SetContentIntent(pendingIntent)
                                        .SetContentTitle(title)
                                        .SetNumber(Count)
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
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.

                log.Write("ACK CreateNotificationChannel NO Surpport");
                return;
            }

            //Context context = Parent as Context;
            //if (context == null)
            //{
            //    log.Write("NAK CreateNotificationChannel IS NullContext");
            //    return;
            //}

            var channel = new NotificationChannel(ChannelId,
                                                  ChannelName,
                                                  Importance)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)(Parent.GetSystemService(Context.NotificationService));
            notificationManager.CreateNotificationChannel(channel);

            log.Write($"CreateNotificationChannel, {ChannelId}, {ChannelName}");
        }
    }
}
