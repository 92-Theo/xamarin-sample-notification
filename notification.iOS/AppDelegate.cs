using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.CloudMessaging;
using Foundation;
using UIKit;
using UserNotifications;

namespace notification.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
                                , IUNUserNotificationCenterDelegate
                                , IMessagingDelegate
    {
        public static AppDelegate Instance { get; private set; }
        private App MainForm;
        private UserNotificationCenterDelegate UNCDelegate = new UserNotificationCenterDelegate();

        #region Override Func
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Instance = this;
            ConfigureNotification();

            global::Xamarin.Forms.Forms.Init();
            MainForm = new App();

            MainForm.AddLocalNotify(NotifyLocal);
            MainForm.AddTokenGet(GetToken);
            MainForm.AppOnForeground += Foreground_Changed;
            MainForm.AppOnBackground += Background_Changed;
            LoadApplication(MainForm);

            
            return base.FinishedLaunching(app, options);
        }


        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            System.Diagnostics.Debug.WriteLine("RegisteredForRemoteNotifications: " + deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            System.Diagnostics.Debug.WriteLine("FailedToRegisterForRemoteNotifications: " + error);
        }
        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            System.Diagnostics.Debug.WriteLine("DidReceiveRemoteNotification: " + userInfo);
            if (userInfo.ObjectForKey(new NSString("aps")) is NSDictionary aps)
            {
                IOSNoticeClickService.Instance.Set((aps[new NSString("category")] as NSString).ToString());
            }
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            //NSString title = ((userInfo["aps"] as NSDictionary)["alert"] as NSDictionary)["title"] as NSString;
            //NSString message = ((userInfo["aps"] as NSDictionary)["alert"] as NSDictionary)["body"] as NSString;

            //System.Diagnostics.Debug.WriteLine("ReceivedRemoteNotification: title(" + title + "), message(" + message + ")");
            // optionally you can send a Xamarin Forms message to 
            // inform the Xamarin Forms Application to handle the notification
            //MessagingCenter.Send(new MessageNotificationReceived()
            //{
            //    Title = title,
            //    Message = message,
            //}, "");
        }
        #endregion

        #region Notification
        private void ConfigureNotification()
        {
            Firebase.Core.App.Configure();

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    Console.WriteLine(granted);
                });

                // For iOS 10 display notification (sent via APNS)
                // UNUserNotificationCenter.Current.Delegate = this;
                UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();
            }
            else
            {
                // iOS 9 <=
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();


            // refreh token
            Firebase.InstanceID.InstanceId.Notifications.ObserveTokenRefresh((sender, e) =>
            {
                var newToken = Firebase.InstanceID.InstanceId.SharedInstance.Token;
                // if you want to send notification per user, use this token
                System.Diagnostics.Debug.WriteLine(newToken);

                Messaging.SharedInstance.Connect((error) =>
                {
                    if (error == null)
                    {
                        Messaging.SharedInstance.Subscribe("/topics/all");
                    }
                    System.Diagnostics.Debug.WriteLine(error != null ? "error occured" : "connect success");
                });
            });
        }
        #endregion


        #region Test
        public void GetToken()
        {
            System.Diagnostics.Debug.WriteLine($"token: {Firebase.InstanceID.InstanceId.SharedInstance.Token}");
        }

        public void NotifyLocal()
        {
            System.Diagnostics.Debug.WriteLine("Call NotifyLocal");
            // center.
            var content = new UNMutableNotificationContent();
            content.Title = "Notification Title";
            content.Subtitle = "Notification Subtitle";
            content.Body = "This is the message body of the notification.";
            content.Sound = UNNotificationSound.Default;
            content.Badge = 1;

            // Fire trigger in twenty seconds
            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(2, false); // more then 0
            var requestID = "sampleRequest";
            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null) // Report error
                    Console.WriteLine($"Error: {err}");
                else   // Report Success
                    Console.WriteLine($"Notification Scheduled: {request}");
            });

        }

        public void WriteForm(string txt)
        {
            MainForm.SetTxtShow(txt);
        }
        #endregion


        #region EventFunc
        public void Foreground_Changed()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                UNUserNotificationCenter.Current.Delegate = UNCDelegate;
        }

        public void Background_Changed()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                UNUserNotificationCenter.Current.Delegate = this;
        }
        #endregion
    }

}
