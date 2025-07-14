using System;
using Enums;
using I2.Loc;
using SettingsContent;
using SettingsContent.SoundContent;
using TMPro;
using UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage;
using UnityEngine;
using WalletContent;

namespace UI.Screens.ShopContent
{
    public class ItemCart : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amount;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _name;
        
        private LanguageChanger _languageChanger;
        private ItemCartScroll _itemCartScroll;
        private string _nameStringTerm;
        
        public DollarValue PricePerUnit{ get; private set; }
        
        public int CurrentAmount { get; private set; }
        
        public ItemType ItemType { get; private set; }
        
        public DollarValue TotalPrice{ get; private set; }

        private void OnDisable()
        {
            _languageChanger.LanguageChanged -= ChangeLanguage;
        }

        public void Init(ItemType itemType, int amount,DollarValue pricePerUnit,DollarValue totalPrice,string name,
            ItemCartScroll itemCartScroll,LanguageChanger languageChanger)
        {
            ItemType = itemType;
            CurrentAmount = amount;
            PricePerUnit = pricePerUnit;
            _amount.text = CurrentAmount.ToString();
            TotalPrice = totalPrice;
            _priceText.text = TotalPrice.ToString();
            _nameStringTerm = name;
            _name.text = LocalizationManager.GetTermTranslation(_nameStringTerm);
            _itemCartScroll = itemCartScroll;
            _languageChanger = languageChanger;
            _languageChanger.LanguageChanged += ChangeLanguage;
        }
        
        public void UpdateAmount(int amount, DollarValue totalPrice)
        {
            CurrentAmount = amount;
            TotalPrice = totalPrice;
            _amount.text = CurrentAmount.ToString();
            _priceText.text = TotalPrice.ToString();
        }
        
        public void IncreaseAmount()
        {
            SoundPlayer.Instance.PlayButtonClick();
            
            if (CurrentAmount >= 9)
                return;

            CurrentAmount++;
            _itemCartScroll.UpdateItemCartInfo(this);
        }

        public void DecreaseAmount()
        {
            SoundPlayer.Instance.PlayButtonClick();
            
            if (CurrentAmount > 1)
            {
                CurrentAmount--;
                _itemCartScroll.UpdateItemCartInfo(this);
            }
            else
            {
                CurrentAmount--;
                
                if (CurrentAmount <= 0)
                {
                    _itemCartScroll.UpdateItemCartInfo(this);
                    _itemCartScroll.DeleteItem(this);
                }
            }
        }

        private void ChangeLanguage()
        {
            Debug.Log("_nameStringTerm " + LocalizationManager.GetTermTranslation(_nameStringTerm));
            Debug.Log("_nameStringTerm " + _nameStringTerm);
            
            _name.text = LocalizationManager.GetTermTranslation(_nameStringTerm);
        }
    }
}