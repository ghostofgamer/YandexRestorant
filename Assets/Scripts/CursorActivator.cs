using InputContent;
using MirraGames.SDK;
using UnityEngine;

public class CursorActivator : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;

    private bool _isCursorActive = true;

    public void SetValue()
    {
        SetValueCursor(!_isCursorActive);
    }

    public void SetValueCursor(bool value)
    {
        if (Application.isMobilePlatform)
        {
            Debug.Log("Application.isMobilePlatform");
            return;
        }

        _playerInput.enabled = !value;
        
        _isCursorActive = value;
        MirraSDK.Device.CursorVisible = value;
        MirraSDK.Device.CursorLock = value ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
            SetValueCursor(!_isCursorActive);
    }
}