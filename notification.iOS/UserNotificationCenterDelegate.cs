using System;
using Firebase.CloudMessaging;
using Foundation;
using UIKit;
using UserNotifications;

namespace notification.iOS
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
                                , IUNUserNotificationCenterDelegate
                                , IMessagingDelegate
    {
        #region Constructors
        public UserNotificationCenterDelegate()
        {
        }
        #endregion

        #region Override Methods
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            System.Diagnostics.Debug.WriteLine($"WillPresentNotification:\n{notification}");

            // Set Notice
            IOSNoticeClickService.Instance.Set(notification.Request.Content.CategoryIdentifier);

            //System.Diagnostics.Debug.WriteLine("Active Notification: {0}", notification);
            //System.Diagnostics.Debug.WriteLine("Date: {0}", notification.Date);
            //System.Diagnostics.Debug.WriteLine("Request Body: {0}", notification.Request.Content.Body);
            //System.Diagnostics.Debug.WriteLine("Request CategoryIdentifier: {0}", notification.Request.Content.CategoryIdentifier);

            
            completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
        }

        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            System.Diagnostics.Debug.WriteLine($"DidReceiveNotificationResponse:\n{response}\n{center}");
            System.Diagnostics.Debug.WriteLine($"{response.Notification.Request.Content.CategoryIdentifier}");
            //// Take action based on Action ID
            //switch (response.ActionIdentifier)
            //{
            //    case "reply":
            //        // Do something
            //        Console.WriteLine("Received the REPLY custom action.");
            //        break;
            //    default:
            //        // Take action based on identifier
            //        if (response.IsDefaultAction)
            //        {
            //            // Handle default action...
            //            Console.WriteLine("Handling the default action.");
            //        }
            //        else if (response.IsDismissAction)
            //        {
            //            // Handle dismiss action
            //            Console.WriteLine("Handling a custom dismiss action.");
            //        }
            //        break;
            //}

            // Inform caller it has been handled
            completionHandler();
        }
        #endregion
    }
}
