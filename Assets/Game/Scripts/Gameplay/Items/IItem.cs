namespace Game2D.Gameplay.Items
{
    public interface IItem
    {
        public IItemData GetItemData { get; }
        public ItemBase GetItem { get; }
    }
}