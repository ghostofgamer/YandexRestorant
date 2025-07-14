using DayNightContent;
using UnityEngine;

namespace RestaurantContent
{
    public class NightLightRestaurant : MonoBehaviour
    {
        [SerializeField] private DayNightCycle _dayNightCycle;
        [SerializeField] private GameObject[] _lights;

        private void OnEnable()
        {
            _dayNightCycle.SetNightLighting += SetActive;
        }

        private void OnDisable()
        {
            _dayNightCycle.SetNightLighting -= SetActive;
        }

        private void SetActive(bool value)
        {
            foreach (var light in _lights)
                light.SetActive(value);
        }
    }
}