namespace Game2D.Gameplay.Inventory
{
    public interface IInventorySaver
    {
        public string IdSave { get; }

        public IInventoryStorage InventoryStorage { get; }
    }
}