using OfflineFantasy.GameCraft.Utility;

namespace OfflineFantasy.GameCraft.ItemContainer
{
    public readonly struct LoseItemEventInfo<TKey, TItemType>
    {
        public readonly uint m_GUID;

        public readonly string m_LostType;

        public readonly TKey m_LostItemID;

        public readonly uint m_LostItemAmount;

        public readonly BaseInventorySlot<TKey, TItemType> m_ModifiedSlot;

        public readonly BaseInventory<TKey, TItemType> m_AffiliatedInventory;

        public LoseItemEventInfo(string _lostType,
                                 TKey _lostItemID,
                                 uint _lostItemAmount,
                                 BaseInventorySlot<TKey, TItemType> _modifiedSlot,
                                 BaseInventory<TKey, TItemType> _affiliatedInventory)
        {
            m_GUID = GameUtility.FormatFunctionTypeGUID();
            m_LostType = _lostType;
            m_LostItemID = _lostItemID;
            m_LostItemAmount = _lostItemAmount;
            m_ModifiedSlot = _modifiedSlot;
            m_AffiliatedInventory = _affiliatedInventory;
        }
    }
}