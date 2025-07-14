using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class FortuneScreen : AbstractScreen
    {
        [SerializeField] private GameObject _popupPrize;
        [SerializeField] private Image _iconPrize;
        [SerializeField] private TMP_Text _valueText;
        
        public event Action FortuneScreenClosed;
        
        public override void CloseScreen()
        {
            FortuneScreenClosed?.Invoke();
            base.CloseScreen();
        }

        public void OpenPopupPrize(Sprite sprite,int value)
        {
            _popupPrize.SetActive(true);
            _iconPrize.sprite = sprite;
            _valueText.text = value.ToString();
        }
    }
}