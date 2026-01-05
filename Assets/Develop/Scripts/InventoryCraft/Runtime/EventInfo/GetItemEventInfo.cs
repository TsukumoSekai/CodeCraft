using OfflineFantasy.GameCraft.Utility;

namespace OfflineFantasy.GameCraft.ItemContainer
{
    public readonly struct GetItemEventInfo<TKey, TItemType>
    {
        public readonly uint m_GUID;

        /// <summary>
        /// 物品来源
        /// </summary>
        public readonly string m_SourceType;

        /// <summary>
        /// 获得物品ID
        /// </summary>
        public readonly TKey m_AcquiredItemID;

        /// <summary>
        /// 获得物品数量
        /// </summary>
        public readonly uint m_AcquiredItemAmount;

        /// <summary>
        /// 变更过的物品栏插槽数组
        /// </summary>
        public readonly BaseInventorySlot<TKey, TItemType> m_ModifiedSlot;

        /// <summary>
        /// 插槽所属物品栏
        /// </summary>
        public readonly BaseInventory<TKey, TItemType> m_AffiliatedInventory;

        public GetItemEventInfo(string _sourceType,
                                TKey _acquiredItemID,
                                uint _acquiredItemAmount,
                                BaseInventorySlot<TKey, TItemType> _modifiedSlot,
                                BaseInventory<TKey, TItemType> _affiliatedInventory)
        {
            m_GUID = GameUtility.FormatFunctionTypeGUID();
            m_SourceType = _sourceType;
            m_AcquiredItemID = _acquiredItemID;
            m_AcquiredItemAmount = _acquiredItemAmount;
            m_ModifiedSlot = _modifiedSlot;
            m_AffiliatedInventory = _affiliatedInventory;
        }
    }
}