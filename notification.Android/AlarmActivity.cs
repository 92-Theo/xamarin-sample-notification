sing System;
using Android.App;
using Android.OS;
using Android.Util;
using Android.Widget;

namespace notification.Droid
{
    //// Theme를 선택해 activity를 출력못하도록 막기
    //[Activity(Label = "SecondActivity", Theme = "@style/MainTheme")]
    //[IntentFilter(new[] { "ALARM" }, Categories = new[] { "android.intent.category.DEFAULT" })]
    //public class AlarmActivity : Activity
    //{
    //    static readonly string TAG = "AlarmActivity";
    //    public static string CurDebugStr { get; private set; }
    //    private static bool isAlarm = false;
    //    public static bool IsAlarm { get { bool val = isAlarm; isAlarm = false; return val; } private set { isAlarm = value; } }

    //    protected override void OnCreate(Bundle savedInstanceState)
    //    {
    //        IsAlarm = true;

    //        Log.Debug(TAG, "OnCreate");
    //        base.OnCreate(savedInstanceState);

    //        CurDebugStr += $"[{TAG}]OnCreate";
    //        StartActivity(typeof(MainActivity));

    //    }
    //    protected override void OnStart()
    //    {
    //        Log.Debug(TAG, "OnStart");
    //        CurDebugStr += $"[{TAG}]OnStart";
    //        base.OnStart();
    //    }
    //    protected override void OnResume()
    //    {
    //        Log.Debug(TAG, "OnResume");
    //        CurDebugStr += $"[{TAG}]OnResume";
    //        base.OnResume();
    //    }
    //    protected override void OnStop()
    //    {
    //        IsAlarm = false;
    //        Log.Debug(TAG, "OnStop");
    //        CurDebugStr += $"[{TAG}]OnStop";
    //        base.OnStop();
    //    }
    //    protected override void OnDestroy()
    //    {
    //        IsAlarm = false;
    //        Log.Debug(TAG, "OnDestroy");
    //        CurDebugStr += $"[{TAG}]OnDestroy";
    //        base.OnDestroy();
    //    }
    //}
}
