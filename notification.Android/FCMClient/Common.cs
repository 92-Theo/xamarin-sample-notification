using System;
using Android.Gms.Common;
using Android.Content;

using notification.Droid.Common;

namespace notification.Droid.FCMClient
{
    public class Common
    {
        static readonly string TAG = "FCMClient.Common";
        static Log log = new Log(TAG);

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
                    log.Write(GoogleApiAvailability.Instance.GetErrorString(resultCode));
                }
                else
                {
                    log.Write("This device is not supported");
                }
                return false;
            }
            else
            {
                log.Write("Google Play Services is available.");
                return true;
            }
        }
    }
}
