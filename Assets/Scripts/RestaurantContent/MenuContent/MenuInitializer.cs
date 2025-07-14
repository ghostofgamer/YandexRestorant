using System.Collections.Generic;
using Enums;
using UI.Screens.ShopContent.ShopPages.PageContents.WorksPage;
using UnityEngine;

namespace RestaurantContent.MenuContent
{
    public class MenuInitializer : MonoBehaviour
    {
        private const string MenuKey = "MenuList";
        
        [SerializeField] private MenuScrollContent _menuScrollContent;
        [SerializeField] private MenuCounter _menuCounter;

        private List<ItemType> _menuList = new List<ItemType>();

        private void OnEnable()
        {
            _menuCounter.ChangeMenuList += ChangeListMenu;
        }

        private void OnDisable()
        {
            _menuCounter.ChangeMenuList -= ChangeListMenu;
        }

        private void Start()
        {
            _menuScrollContent.Init();
            _menuList = LoadMenu();

            foreach (var t in _menuList)
                _menuScrollContent.AddItem(t);
        }

        private void ChangeListMenu(List<ItemType> menuList)
        {
            SaveMenu(menuList);
        }

        private void SaveMenu(List<ItemType> menuList)
        {
            List<string> stringList = new List<string>();
            foreach (var item in menuList)
            {
                stringList.Add(item.ToString());
            }

            string json = JsonUtility.ToJson(new Serialization<string>(stringList));
            PlayerPrefs.SetString(MenuKey, json);
            PlayerPrefs.Save();
        }

        private List<ItemType> LoadMenu()
        {
            if (PlayerPrefs.HasKey(MenuKey))
            {
                string json = PlayerPrefs.GetString(MenuKey);
                List<string> stringList = JsonUtility.FromJson<Serialization<string>>(json).target;
                List<ItemType> menuList = new List<ItemType>();
                foreach (var str in stringList)
                {
                    menuList.Add((ItemType)System.Enum.Parse(typeof(ItemType), str));
                }

                return menuList;
            }

            return new List<ItemType>();
        }
        
        [System.Serializable]
        public class Serialization<T>
        {
            public List<T> target;

            public Serialization(List<T> target)
            {
                this.target = target;
            }
        }
    }
}