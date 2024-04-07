using Game2D.Gameplay.Items;

namespace Game2D.Gameplay.Interactions
{
    public interface IInteractableItem : IItem
    {
        public void Interacting();

        public void InteractingAlt();

        public void DropingItem();
    }
}