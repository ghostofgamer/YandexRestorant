using TMPro;
using UnityEngine;

namespace EnergyContent
{
    public class EnergyViewer : MonoBehaviour
    {
        [SerializeField] private Energy _energy;
        [SerializeField] private TMP_Text _energyCountText;

        private void OnEnable()
        {
            _energy.EnergyValueChanged += ShowEnergy;
        }

        private void OnDisable()
        {
            _energy.EnergyValueChanged -= ShowEnergy;
        }

        private void ShowEnergy(int value)
        {
            _energyCountText.text = value.ToString();
        }
    }
}
