using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace IngredientsContent
{
    public class IngredientsViewer : MonoBehaviour
    {
        [SerializeField] private Image _defaultImage;
        [SerializeField] private Image _backGroundImage;

        public ItemType ItemType;

        public void SetItemType(ItemType itemType)
        {
            ItemType = itemType;
        }
        
        public void SetValueBackground(bool value)
        {
            _backGroundImage.gameObject.SetActive(value);
        }

        public void SetValue(bool value)
        {
            gameObject.SetActive(value);
        }

        public void SetDefault(Sprite sprite)
        {
            _defaultImage.sprite = sprite;
        }

        public void SetDefaultColor(Color color )
        {
            _defaultImage.color = color;
        }

        /*public void ResetDefaultScale()
        {
            _defaultImage.transform.localScale = Vector3.one;
        }*/

        public void SetOutlineBackground(bool value, Sprite sprite)
        {
            _backGroundImage.sprite = sprite;
            _backGroundImage.gameObject.SetActive(value);
        }

        public void DeactivationOutline()
        {
            _backGroundImage.gameObject.SetActive(false);
        }
    }
}