using Game2D.Gameplay.Inventory;

namespace Game2D.Gameplay.Character
{
    public interface ICharacter
    {
        IInventoryStorage InventoryStorage { get; }
    }
}