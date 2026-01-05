namespace OfflineFantasy.GameCraft.Item
{
    public interface IItemInfoFactory<TKey, TItemType>
    {
        public IItemInfo GenerateIItemInfo(TKey _id, TItemType _itemType, uint _amount);
    }
}