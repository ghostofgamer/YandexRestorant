using UnityEngine;

namespace ItemContent
{
    public class ActivityItem : MonoBehaviour
    {
        public bool IsActive { get; private set; } = true;

        public void SetValue(bool value)
        {
            IsActive = value;
            gameObject.SetActive(value);
            Debug.Log("ВЫКЛЮЧАЕЕЕЕМ " + value);
        }
    }
}
