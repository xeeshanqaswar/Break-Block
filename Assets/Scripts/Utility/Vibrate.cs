using UnityEngine;

public static class Vibrate
{
    #region FIELDS DECLERATION

    #if UNITY_ANDROID && !UNITY_EDITOR
        public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService","vibrator");
    #else
        public static AndroidJavaClass unityPlayer;
        public static AndroidJavaObject currentActivity ;
        public static AndroidJavaObject vibrator;
    #endif

    #endregion

    public static void PerformeVibration(long milliseconds = 250)
    {
        if (IsAndroid())
        {
            vibrator.Call("vibrate", milliseconds);
        }
        else
        {
            //Handheld.Vibrate();
        }
    }

    public static void CancleVibration()
    {
        if (IsAndroid())
        {
            vibrator.Call("cancle");
        }
    }

    public static bool IsAndroid()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            return true;
        #else
            return false;
        #endif
    }
}
