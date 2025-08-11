using UnityEngine;

public class WindowsCounter : MonoBehaviour
{
    public int CurrentValue { get; private set; }

    public void IncreaseValue()
    {
        CurrentValue++;
        ShowValue();
    }

    public void DecreaseValue()
    {
        CurrentValue = Mathf.Max(CurrentValue - 1, 0);
        ShowValue();
    }

    private void ShowValue()
    {
        Debug.Log("Колличество открытых окон: " + CurrentValue);
    }
}