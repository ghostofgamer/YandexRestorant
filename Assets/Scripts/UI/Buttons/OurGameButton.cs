using UnityEngine;

namespace UI.Buttons
{
    public class OurGameButton : AbstractButton
    {
        [SerializeField] private string _link;
        
        public override void OnClick()
        {
            Application.OpenURL(_link);
        }
    }
}