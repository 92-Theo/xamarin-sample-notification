using System;
namespace notification
{
    #region Delegate

    public delegate void DefaultEventHandler();

    #endregion

    #region Class
    public class Logger
    {
        public static Logger Instance;

        static Logger() {
            Instance = new Logger();
        }


        public void WriteDebug(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }
        public void WriteDebug(string tag, string msg)
        {
            WriteDebug($"[{tag}] {msg}");
        }


        public static void CmWrite(string msg)
        {
#if DEBUG
            Instance?.WriteDebug(msg);
#endif
        }
        public static void CmWrite(string tag, string msg)
        {
#if DEBUG
            Instance?.WriteDebug(tag, msg);
#endif
        }

    }
    #endregion
}
