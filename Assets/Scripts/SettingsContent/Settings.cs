using UnityEngine;
using UnityEngine.UI;

namespace SettingsContent
{
    public class Settings : MonoBehaviour
    {
        void Start()
        {
            
        }

        public void SetValueSound(bool value)
        {
            // Debug.Log("ЗВУК " + value);
        }

        public void SetValueMusic(bool value)
        {
            // Debug.Log("Музыка " + value);
        }

        public void SetValueSensa(float value)
        {
            // Debug.Log("Установленное значение сенсы: " + value);
        }
    }
}