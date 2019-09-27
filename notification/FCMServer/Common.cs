using System;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Forms.Internals;

namespace notification.FCMServer
{
    public class Common
    {
        static readonly string TAG = "FCMServer.Common";
        private const string FORMAT = "{{ \"registration_ids\": [ {0} ], \"notification\": {{\"title\":\"{1}\",\"body\":\"{2}\"}}, \"delay_while_idle\" : false, \"priority\" : \"high\" }}";

        public Common()
        {
        }


        public static void SendPush(string apiKey, string deviceId, string notifiTitle, string notifiBody)
        {
            try
            {
                WebRequest req;
                req = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                req.Method = "post";
                req.ContentType = "application/json";
                req.Headers.Add(string.Format("Authorization: key={0}", apiKey));
                deviceId = deviceId.Replace(",", "\",\"");
                deviceId = "\"" + deviceId + "\"";
                string postData = string.Format(FORMAT, deviceId, notifiTitle, notifiBody);
                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                req.ContentLength = byteArray.Length;
                Stream dataStream = req.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse res = req.GetResponse();
                dataStream = res.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);
                String sResponseFromServer = tReader.ReadToEnd();
                tReader.Close();
                dataStream.Close();
                res.Close();
            }
            catch (Exception ex)
            {
                // Log.Error(TAG, "NAK SendPush {0}", ex);
            }
        }
    }
}