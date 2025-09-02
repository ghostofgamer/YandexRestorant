using SaveContent;
using UI.Buttons;

namespace Test
{
    public class ClearDataAll : AbstractButton
    {
        public override void OnClick()
        {
            StorageHelper.DeleteAll(true);
        }
    }
}