using System;
using Firebase.CloudMessaging;
using Foundation;
using UIKit;
using UserNotifications;
using notification.iOS.Services;
using notification.Services;

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
        private static readonly string TAG = "iOS.AppDelegate";

        public static AppDelegate Instance { get; private set; }
        private App mainApp;
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
            mainApp = new App();

            mainApp.AddLocalNotify(NotifyLocal);
            mainApp.AddTokenGet(GetToken);
            mainApp.AppOnForeground += Foreground_Changed;
            mainApp.AppOnBackground += Background_Changed;
            LoadApplication(mainApp);
            
            return base.FinishedLaunching(app, options);
        }


        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
        }
        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            if (CmFindNoticeInfo(userInfo))
            {
                CmRequestNoticePage();
            }
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
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

                IOSNoticeService.Instance.SetDeviceToken(newToken);
                CmRefreshToken();

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


        public void GetToken()
        {
            CmWriteForm($"token: {Firebase.InstanceID.InstanceId.SharedInstance.Token}");
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


        public static void CmWriteForm(string txt)
        {
            Instance?.mainApp.SetTxtShow(txt);
        }
        public static void CmRequestNoticePage()
        {
            Instance?.mainApp.RequestNoticePage();
        }
        public static void CmRefreshToken()
        {
            Instance?.mainApp.RefreshToken();
        }
        public static bool CmFindNoticeInfo(NSDictionary userInfo)
        {
            if (userInfo.ObjectForKey(new NSString("aps")) is NSDictionary aps)
            {
                string myvalue = userInfo.ObjectForKey(new NSString("mykey")).ToString();
                if (!myvalue.Equals(""))
                {
                    NoticeInfo info;
                    info.action = (aps[new NSString("category")] as NSString).ToString();
                    info.data = myvalue;
                    CmRequestNoticePage();
                    return true;
                }
                // to get body : (aps.ObjectForKey(new NSString("alert"))[new NSString("body")] as NSString).ToString()
            }

            return false;
        }

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
