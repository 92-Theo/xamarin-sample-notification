using System;

namespace notification.Droid.Common
{
	public class Log
	{
		private readonly string TAG;
		public Log(string tag)
		{
			TAG = tag;
		}


		public void Write(string msg)
		{
#if DEBUG
            WriteDebug(msg);
#endif
		}

		private void WriteDebug(string msg)
		{
			Android.Util.Log.Debug(TAG, msg);
		}
	}
}
