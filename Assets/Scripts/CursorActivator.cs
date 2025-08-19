using InputContent;
using MirraGames.SDK;
using UnityEngine;

public class CursorActivator : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private WindowsCounter _windowsCounter;

    private bool _isCursorActive = true;

    public void SetValue()
    {
        SetValueCursor(!_isCursorActive);
    }

    public void SetValueCursor(bool value)
    {
        Debug.Log("SetValueCursor" + value);

        if (!value && _windowsCounter.CurrentValue > 0)
        {
            Debug.Log("Есть открытые окна " + _windowsCounter.CurrentValue);
            return;
        }

        if (Application.isMobilePlatform)
        {
            Debug.Log("Application.isMobilePlatform");
            return;
        }

        Debug.Log("Value Cursor Changed");
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