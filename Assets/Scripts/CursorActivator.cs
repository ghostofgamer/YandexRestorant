using MirraGames.SDK;
using UnityEngine;

public class CursorActivator : MonoBehaviour
{
    private bool _isCursorActive = true;
    
    public void SetValueCursor(bool value)
    {
        if (Application.isMobilePlatform)
        {
            Debug.Log("Application.isMobilePlatform");
            return;
        }

        _isCursorActive = value;
        MirraSDK.Device.CursorVisible = value;
        MirraSDK.Device.CursorLock = value ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            SetValueCursor(!_isCursorActive);
    }
}