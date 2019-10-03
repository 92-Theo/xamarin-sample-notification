using System;
using Android.Gms.Common;
using Android.Content;

using Firebase.Iid;

namespace notification.Droid.Services
{
    public class Common
    {
        static readonly string TAG = "Services.Common";

        public Common()
        {
        }

        public static bool IsGoogleApiAvailability(Context context)
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(context);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    Logger.CmWrite(TAG, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                }
                else
                {
                    Logger.CmWrite(TAG, "This device is not supported");
                }
                return false;
            }
            else
            {
                Logger.CmWrite(TAG, "Google Play Services is available.");
                return true;
            }
        }

        public static string GetDeviceToken()
        {
            return FirebaseInstanceId.Instance.Token;
        }
    }
}
