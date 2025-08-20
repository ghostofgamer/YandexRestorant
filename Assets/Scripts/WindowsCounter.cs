using System.Collections.Generic;
using TMPro;
using UI.Screens;
using UnityEngine;

public class WindowsCounter : MonoBehaviour
{
    [SerializeField]private TMP_Text _valueText;
    
    private List<AbstractScreen> _openedWindows = new List<AbstractScreen>();
    
    public int CurrentValue => _openedWindows.Count;

    private void Start()
    {
        ShowValue();
    }

    public bool TryAddWindow(AbstractScreen window)
    {
        if (_openedWindows.Contains(window))
        {
            Debug.LogWarning($"Окно {window.name} уже открыто!");
            return false;
        }

        _openedWindows.Add(window);
        ShowValue();
        return true;
    }

    public bool TryRemoveWindow(AbstractScreen window)
    {
        if (!_openedWindows.Contains(window))
        {
            Debug.LogWarning($"Окно {window.name} не найдено в списке открытых окон!");
            return false;
        }

        _openedWindows.Remove(window);
        ShowValue();
        return true;
    }
    
    /*public void IncreaseValue()
    {
        CurrentValue++;
        ShowValue();
    }

    public void DecreaseValue()
    {
        CurrentValue = Mathf.Max(CurrentValue - 1, 0);
        ShowValue();
    }*/

    private void ShowValue()
    {
        _valueText.text = CurrentValue.ToString();
        Debug.Log("Колличество открытых окон: " + CurrentValue);
    }
}