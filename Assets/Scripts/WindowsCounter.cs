using TMPro;
using UnityEngine;

public class WindowsCounter : MonoBehaviour
{
    [SerializeField]private TMP_Text _valueText;
    
    public int CurrentValue { get; private set; }

    private void Start()
    {
        ShowValue();
    }

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
        _valueText.text = CurrentValue.ToString();
        Debug.Log("Колличество открытых окон: " + CurrentValue);
    }
}