using Enums;
using I2.Loc;
using SoContent;
using TMPro;
using UI.Screens.ShopContent.ShopPages.PageContents.WorksPage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MenuUIContent
{
    public abstract class AbstractUIMenuItem : MonoBehaviour
    {
        [SerializeField] protected MenuScrollContent _menuScrollContent;

        [SerializeField] private ItemType _itemType;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _nameItemText;

        protected ItemConfig ItemConfig;

        public ItemType ItemType => _itemType;

        public virtual void Init(ItemsConfig itemsConfig)
        {
            ItemConfig = itemsConfig.GetItemConfig(_itemType);

            if (ItemConfig != null)
            {
                _image.sprite = ItemConfig.Sprite;
                _nameItemText.text = LocalizationManager.GetTermTranslation(ItemConfig.Term);
            }
            else
            {
                Debug.Log("Отсутсвует конфиг ");
            }
        }
    }
}