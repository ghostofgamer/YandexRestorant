using Io.AppMetrica;
using UnityEngine;

public static class AppMetricaActivator
{
    private static readonly string _playerPrefsKey = "AppMetricaActivator-IsFirstLaunch";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ActivateAppMetrica()
    {
        AppMetricaConfig appMetricaConfig = new("37618e64-e1c4-4382-9ba2-8c4bdcd79d03")
        {
            CrashReporting = true,
            SessionTimeout = 10,
            LocationTracking = false,
            Logs = false,
            FirstActivationAsUpdate = !IsFirstLaunch(),
            DataSendingEnabled = true,
        };

        AppMetrica.Activate(appMetricaConfig);
    }

    private static bool IsFirstLaunch()
    {
        if (PlayerPrefs.HasKey(_playerPrefsKey))
        {
            return false;
        }
        
        PlayerPrefs.SetInt(_playerPrefsKey, 1);
        PlayerPrefs.Save(); 
        return true;
        
        
        
        
        
        /*if (PlayerPrefs.HasKey(_playerPrefsKey))
        {
            return true;
        }

        PlayerPrefs.SetString(_playerPrefsKey, string.Empty);
        return false;*/
    }
}