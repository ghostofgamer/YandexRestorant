using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _targetColor;
    [SerializeField] private Image _image;

    public void SetDefaultColor()
    {
        _image.color = _defaultColor;
    }

    public void SetTargetColor()
    {
        _image.color = _targetColor;
    }
}