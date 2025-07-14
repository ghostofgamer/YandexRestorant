using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class CongratulationMysteryBoxScreen : AbstractScreen
    {
        [SerializeField] private Image _imageIcon;
        [SerializeField] private TMP_Text _valueText;

        public void Init(Sprite sprite, int value)
        {
            _imageIcon.sprite = sprite;
            _valueText.text = value.ToString();
        }
    }
}