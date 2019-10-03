using System;
using Firebase.CloudMessaging;
using Foundation;
using UIKit;
using UserNotifications;
using notification.iOS.Services;

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
            completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
        }

        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            if (AppDelegate.CmFindNoticeInfo(response.Notification.Request.Content.UserInfo))
            {
                AppDelegate.CmRequestNoticePage();
            }

            completionHandler();
        }
        #endregion
    }
}
