using UnityEngine;

namespace TaskContent
{
    public abstract class GameTaskAbstract : MonoBehaviour
    {
        public abstract void ActivateTask();
        
        public abstract void CompletedTask();
    }
}