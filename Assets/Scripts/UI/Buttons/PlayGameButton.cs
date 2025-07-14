using Io.AppMetrica;
using UnityEngine;

namespace UI.Buttons
{
    public class PlayGameButton : AbstractButton
    {
        [SerializeField] private GameObject _loadScreen;
        
        public override void OnClick()
        {
            _loadScreen.SetActive(false);
            AppMetrica.ReportEvent("StartGame");
        }
    }
}