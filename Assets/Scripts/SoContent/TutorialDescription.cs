using Enums;
using UnityEngine;

namespace SoContent
{
    [CreateAssetMenu(fileName = "TutorialDescriptions", menuName = "Tutorial/TutorialDescriptions", order = 1)]
    public class TutorialDescription : ScriptableObject
    {
        [System.Serializable]
        public class Description
        {
            public TutorialType type;
            public string text;
        }

        public Description[] descriptions;
    }
}