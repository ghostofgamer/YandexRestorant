using PlayerContent;

namespace Interfaces
{
    public interface IInteractable
    {
        void EnableOutline();
        
        void DisableOutline();
        
        void Action(PlayerInteraction playerInteraction);
    }
}