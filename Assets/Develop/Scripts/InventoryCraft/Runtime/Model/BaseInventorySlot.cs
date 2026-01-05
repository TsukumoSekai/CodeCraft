using System;
using OfflineFantasy.GameCraft.Item;
using OfflineFantasy.GameCraft.Utility.Event;

namespace OfflineFantasy.GameCraft.ItemContainer
{
    /// <summary>
    /// 背包槽位基类
    /// </summary>
    [System.Serializable]
    public class BaseInventorySlot<TKey, TItemType>
    {
        #region 私有字段

        private BaseInventory<TKey, TItemType> m_Inventory;

        #endregion

        #region 公共字段

        [Newtonsoft.Json.JsonIgnore]
        public bool m_IsNew;

        /// <summary>
        /// 获得物品事件
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public Action<GetItemEventInfo<TKey, TItemType>> m_OnGetItemEvent;

        /// <summary>
        /// 失去物品事件
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public Action<LoseItemEventInfo<TKey, TItemType>> m_OnLostItemEvent;

        #endregion

        #region 属性

        /// <summary>
        /// 所属物品栏
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public BaseInventory<TKey, TItemType> Inventory => m_Inventory;

        /// <summary>
        /// 槽位物品数量
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public uint ItemAmount => ItemInfo != null ? ItemInfo.Amount : 0;

        /// <summary>
        /// 槽位最大堆叠数量
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public virtual uint MaxAmountInSlot => ItemInfo.MaxAmountInSlot;

        /// <summary>
        /// 槽位剩余可堆叠数量
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public uint RemainAmountInSlot => MaxAmountInSlot - ItemAmount;

        /// <summary>
        /// 槽位内物品信息
        /// </summary>
        public IItemInfo<TKey, TItemType> ItemInfo { set; get; }

        #endregion

        #region 公共静态方法

        /// <summary>
        /// 构造工厂
        /// </summary>
        /// <param name="_itemInfo"></param>
        /// <returns></returns>
        public static BaseInventorySlot<TKey, TItemType> Create(IItemInfo<TKey, TItemType> _itemInfo, string _sourceType = null)
        {
            BaseInventorySlot<TKey, TItemType> inventorySlot = new BaseInventorySlot<TKey, TItemType>();
            inventorySlot.ItemInfo = _itemInfo;
            inventorySlot.m_IsNew = _sourceType != null && _sourceType != GameResourcesSourceType.Internal;

            return inventorySlot;
        }

        #endregion

        #region 公共方法

        public virtual void Initialize(BaseInventory<TKey, TItemType> _inventory)
        {
            m_Inventory = _inventory;

            if (ItemInfo != null)
                ItemInfo.Initialize(this);
        }

        /// <summary>
        /// 添加物品
        /// </summary>
        /// <param name="_amount"></param>
        /// <returns></returns>
        public virtual bool Add(uint _amount, string _sourceType)
        {
            if (RemainAmountInSlot >= _amount)
            {
                ItemInfo.Add(_amount, _sourceType);

                OnAdd(_amount, _sourceType);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 移除物品
        /// </summary>
        /// <param name="_amount"></param>
        /// <returns></returns>
        public bool Remove(uint _amount, string _lostType)
        {
            bool result = ItemInfo.Remove(_amount, _lostType);

            if (result)
                OnRemove(_amount, _lostType);

            return result;
        }

        public void OnAdd(uint _amount, string _sourceType)
        {
            GetItemEventInfo<TKey, TItemType> getItemEventInfo = new GetItemEventInfo<TKey, TItemType>(_sourceType, ItemInfo.ItemDataID, _amount, this, m_Inventory);

            m_OnGetItemEvent?.Invoke(getItemEventInfo);
            m_Inventory?.OnGetItem(getItemEventInfo);
            //m_Inventory?.OnGetItem(ItemInfo.ItemDataID, _amount, this, _sourceType);

            EventCenter<InventoryEventType>.Broadcast(InventoryEventType.GetItem, getItemEventInfo);
        }

        public void OnRemove(uint _amount, string _lostType)
        {
            LoseItemEventInfo<TKey, TItemType> loseItemEventInfo = new LoseItemEventInfo<TKey, TItemType>(_lostType, ItemInfo.ItemDataID, _amount, this, m_Inventory);

            m_OnLostItemEvent?.Invoke(loseItemEventInfo);
            m_Inventory?.OnLoseItem(loseItemEventInfo);
            //m_Inventory?.OnLoseItem(ItemInfo.ItemDataID, _amount, this, _lostType);

            EventCenter<InventoryEventType>.Broadcast(InventoryEventType.LoseItem, loseItemEventInfo);
        }

        #endregion
    }

    ///// <summary>
    ///// ID顺序
    ///// </summary>
    //public class InventorySlotIDComparer : IComparer<BaseInventorySlot>
    //{
    //    public int Compare(BaseInventorySlot _left, BaseInventorySlot _right)
    //    {
    //        return _left.ItemInfo.ItemID.CompareTo(_right.ItemInfo.ItemID);
    //    }
    //}

    ///// <summary>
    ///// 类型顺序
    ///// </summary>
    //public class InventorySlotTypeComparer : IComparer<BaseInventorySlot>
    //{
    //    public int Compare(BaseInventorySlot _left, BaseInventorySlot _right)
    //    {
    //        int result = ((int)_left.ItemInfo.ItemData.m_ItemType).CompareTo((int)_right.ItemInfo.ItemData.m_ItemType);

    //        if (result == 0)
    //            result = _left.ItemInfo.ItemID.CompareTo(_right.ItemInfo.ItemID);

    //        return result;
    //    }
    //}

    ///// <summary>
    ///// 价格顺序
    ///// </summary>
    //public class InventorySlotPriceComparer : IComparer<BaseInventorySlot>
    //{
    //    public int Compare(BaseInventorySlot _left, BaseInventorySlot _right)
    //    {
    //        int result = -_left.ItemInfo.ItemData.SellPrice.CompareTo(_right.ItemInfo.ItemData.SellPrice);

    //        if (result == 0)
    //            result = _left.ItemInfo.ItemID.CompareTo(_right.ItemInfo.ItemID);

    //        return result;
    //    }
    //}

    ///// <summary>
    ///// 获取时间顺序
    ///// </summary>
    //public class InventorySlotGetTimeComparer : IComparer<BaseInventorySlot>
    //{
    //    public int Compare(BaseInventorySlot _left, BaseInventorySlot _right)
    //    {
    //        return -_left.ItemInfo.GetTime.CompareTo(_right.ItemInfo.GetTime);
    //    }
    //}

    //public class InventorySlotQualityComparer : IComparer<BaseInventorySlot>
    //{
    //    public int Compare(BaseInventorySlot _left, BaseInventorySlot _right)
    //    {
    //        int result = ((int)_left.ItemInfo.ItemData.m_ItemQualityType).CompareTo((int)_right.ItemInfo.ItemData.m_ItemQualityType);

    //        if (result == 0)
    //            result = _left.ItemInfo.ItemID.CompareTo(_right.ItemInfo.ItemID);

    //        return result;
    //    }
    //}

    //public class InventorySlotEquipmentLevelComparer : IComparer<BaseInventorySlot>
    //{
    //    public int Compare(BaseInventorySlot _left, BaseInventorySlot _right)
    //    {
    //        if (_left.ItemInfo is Equipment && _right.ItemInfo is Equipment)
    //        {
    //            int result = (_left.ItemInfo as Equipment).EquipmentData.m_EquipmentLevel.CompareTo((_right.ItemInfo as Equipment).EquipmentData.m_EquipmentLevel);

    //            if (result != 0)
    //                return result;
    //        }

    //        return _left.ItemInfo.ItemID.CompareTo(_right.ItemInfo.ItemID);
    //    }
    //}
}