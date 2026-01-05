namespace OfflineFantasy.GameCraft.ItemContainer
{
    //TODO:事件发送待补全
    public enum InventoryEventType
    {
        GetItem,
        LoseItem,
        CreatItemSlot,
        DestroyItemSlot,
        ItemInfoUpdate,
    }

    public static class InventoryUtility
    {
        /// <summary>
        /// 检定插槽内是否有物品
        /// </summary>
        /// <param name="_slot"></param>
        /// <returns></returns>
        public static bool HasItem<TKey, TItemType>(this BaseInventorySlot<TKey, TItemType> _slot)
        {
            return _slot != null && _slot.ItemInfo != null && _slot.ItemAmount > 0;
        }

        /// <summary>
        /// 获取指定物品在单个槽位内的最大堆叠数量
        /// </summary>
        /// <param name="_itemID"></param>
        /// <returns></returns>
        public static uint GetMaxAmountInSlotByID<KeyType>(KeyType _itemID)
        {
            return uint.MaxValue;
            //switch (ItemData.DataDict[_itemID].m_ItemType)
            //{
            //    case ItemType.Equipment:
            //    case ItemType.PetEgg:
            //        return 1;
            //    default:
            //        return 999;
            //}
        }

        //public static readonly InventorySlotComparer m_InventorySlotComparer = new InventorySlotComparer();

        ///// <summary>
        ///// 物品栏插槽比较器
        ///// </summary>
        //public class InventorySlotComparer : IComparer<BaseInventorySlot>
        //{
        //    public int Compare(BaseInventorySlot x, BaseInventorySlot y)
        //    {
        //        if (x.ItemInfo == null)
        //            return 1;

        //        if (y.ItemInfo == null)
        //            return -1;

        //        int result = 0;

        //        //是否为新物品
        //        if (x.m_IsNew)
        //        {
        //            if (y.m_IsNew)
        //                result = 0;
        //            else
        //                result = -1;
        //        }
        //        else if (y.m_IsNew)
        //            result = 1;

        //        if (result != 0)
        //            return result;

        //        //物品类型
        //        result = GetItemTypeSortingIndex(x.ItemInfo.ItemData.m_ItemType).CompareTo(GetItemTypeSortingIndex(y.ItemInfo.ItemData.m_ItemType));

        //        if (result != 0)
        //            return result;

        //        //品质
        //        result = ((int)y.ItemInfo.ItemData.m_ItemQualityType).CompareTo((int)x.ItemInfo.ItemData.m_ItemQualityType);

        //        if (result != 0)
        //            return result;

        //        ////装备强化等级
        //        //if (x.ItemInfo is Equipment && y.ItemInfo is Equipment)
        //        //{
        //        //    result = (y.ItemInfo as Equipment).m_ReinforcementLevel.CompareTo((x.ItemInfo as Equipment).m_ReinforcementLevel);

        //        //    if (result != 0)
        //        //        return result;
        //        //}

        //        //ID
        //        result = x.ItemInfo.ItemID.CompareTo(y.ItemInfo.ItemID);

        //        if (result != 0)
        //            return result;

        //        //获取时间
        //        return x.ItemInfo.GetTime.CompareTo(y.ItemInfo.GetTime);
        //    }
        //}

        ///// <summary>
        ///// 根据物品类型获取排序权重
        ///// </summary>
        ///// <param name="_itemType"></param>
        ///// <returns></returns>
        //public static int GetItemTypeSortingIndex(ItemType _itemType)
        //{
        //    //switch (_itemType)
        //    //{
        //    //    case ItemType.Equipment:
        //    //        return 1;
        //    //    case ItemType.Material:
        //    //        return 2;
        //    //    case ItemType.Consumables:
        //    //        return 0;
        //    //}

        //    return 0;
        //}

        #region 转移

        ///// <summary>
        ///// 在背包间转移指定数量的物品
        ///// </summary>
        ///// <param name="_sender"></param>
        ///// <param name="_receiver"></param>
        ///// <param name="_itemID"></param>
        ///// <param name="_transferAmount"></param>
        ///// <returns></returns>
        //public static bool TransferItemWithSpecifiedCountBetweenInventory(BaseInventory _sender, BaseInventory _receiver,
        //                                                                  uint _itemID, uint _transferAmount,
        //                                                                  string _lostType = null, string _sourceType = null)
        //{
        //    if (!_sender.CheckHasItem(_itemID, _transferAmount) ||
        //        !_receiver.CheckHasEmptyCapacity(_itemID, _transferAmount))
        //        return false;

        //    _sender.RemoveItem(_itemID, _transferAmount, _lostType);
        //    _receiver.AddItem(_itemID, _transferAmount, _sourceType);

        //    return true;
        //}

        ///// <summary>
        ///// 在背包间转移指定数量的物品
        ///// </summary>
        ///// <param name="_sender"></param>
        ///// <param name="_receiver"></param>
        ///// <param name="_item"></param>
        ///// <param name="_transferAmount"></param>
        ///// <returns></returns>
        //public static bool TransferItemWithSpecifiedCountBetweenInventory(BaseInventory _sender, BaseInventory _receiver,
        //                                                                  IItemInfo _item, uint _transferAmount,
        //                                                                  string _lostType = null, string _sourceType = null)
        //{
        //    if (!_sender.CheckHasItem(_item.ItemID, _item.Amount) ||
        //        !_receiver.CheckHasEmptyCapacity(_item.ItemID, _item.Amount))
        //        return false;

        //    _sender.RemoveItem(_item, _transferAmount, _lostType);
        //    _receiver.AddItem(_item, _transferAmount, _sourceType);

        //    return true;
        //}

        ///// <summary>
        ///// 在背包间转移所有数量的物品
        ///// </summary>
        ///// <param name="_sender"></param>
        ///// <param name="_receiver"></param>
        ///// <param name="_item"></param>
        ///// <returns></returns>
        //public static bool TransferItemWithAllCountBetweenInventory(BaseInventory _sender, BaseInventory _receiver,
        //                                                            IItemInfo _item, string _lostType = null, string _sourceType = null)
        //{
        //    if (!_sender.CheckHasItem(_item.ItemID, _item.Amount) ||
        //        !_receiver.CheckHasEmptyCapacity(_item.ItemID, _item.Amount))
        //        return false;

        //    _sender.RemoveItem(_item, _lostType);
        //    _receiver.AddItem(_item, _sourceType);

        //    return true;
        //}

        ///// <summary>
        ///// 在背包间尽可能多的转移物品
        ///// </summary>
        ///// <param name="_sender"></param>
        ///// <param name="_receiver"></param>
        ///// <param name="_itemID"></param>
        ///// <param name="_transferAmount"></param>
        ///// <returns></returns>
        //public static void TransferItemWithMinCountBetweenInventory(BaseInventory _sender, BaseInventory _receiver,
        //                                                            uint _itemID, uint _transferAmount,
        //                                                            string _lostType = null, string _sourceType = null)
        //{
        //    _transferAmount = (uint)Mathf.Min(_transferAmount,
        //                                      (int)_sender.GetSpecifiedItemCount(_itemID),
        //                                      (int)_receiver.GetCanAddItemAmount(_itemID));

        //    _sender.RemoveItem(_itemID, _transferAmount, _lostType);
        //    _receiver.AddItem(_itemID, _transferAmount, _sourceType);
        //}

        ///// <summary>
        ///// 在背包间尽可能多的转移物品
        ///// </summary>
        ///// <param name="_sender"></param>
        ///// <param name="_receiver"></param>
        ///// <param name="_item"></param>
        ///// <param name="_transferAmount"></param>
        ///// <returns></returns>
        //public static void TransferItemWithMinCountBetweenInventory(BaseInventory _sender, BaseInventory _receiver,
        //                                                            IItemInfo _item, uint _transferAmount,
        //                                                            string _lostType = null, string _sourceType = null)
        //{
        //    _transferAmount = (uint)Mathf.Min(_transferAmount,
        //                                      (int)_sender.GetSpecifiedItemCount(_item.ItemID),
        //                                      (int)_receiver.GetCanAddItemAmount(_item.ItemID));

        //    _sender.RemoveItem(_item, _transferAmount, _lostType);
        //    _receiver.AddItem(_item, _transferAmount, _sourceType);
        //}

        ///// <summary>
        ///// 在背包间尽可能多的转移物品
        ///// </summary>
        ///// <param name="_sender"></param>
        ///// <param name="_receiver"></param>
        ///// <param name="_item"></param>
        ///// <returns></returns>
        //public static void TransferItemWithMinCountBetweenInventory(BaseInventory _sender, BaseInventory _receiver,
        //                                                            IItemInfo _item, string _lostType = null, string _sourceType = null)
        //{
        //    uint transferAmount = (uint)Mathf.Min((int)_sender.GetSpecifiedItemCount(_item.ItemID),
        //                                          (int)_receiver.GetCanAddItemAmount(_item.ItemID));

        //    _sender.RemoveItem(_item, transferAmount, _lostType);
        //    _receiver.AddItem(_item, transferAmount, _sourceType);
        //}

        ///// <summary>
        ///// 在插槽间转移指定数量的物品
        ///// </summary>
        ///// <param name="_sender"></param>
        ///// <param name="_receiver"></param>
        ///// <param name="_transferAmount"></param>
        ///// <param name="_lostType"></param>
        ///// <param name="_sourceType"></param>
        ///// <returns></returns>
        //public static bool TransferItemWithSpecifiedCountBetweenSlot(BaseInventorySlot _sender, BaseInventorySlot _receiver,
        //                                                             uint _transferAmount, string _lostType,
        //                                                             string _sourceType)
        //{
        //    if (_sender.ItemInfo.ItemID != _receiver.ItemInfo.ItemID)
        //        return false;

        //    if (_sender.ItemAmount < _transferAmount || _receiver.RemainAmountInSlot < _transferAmount)
        //        return false;

        //    _sender.Remove(_transferAmount, _lostType);
        //    _receiver.Add(_transferAmount, _sourceType);

        //    return true;
        //}

        ///// <summary>
        ///// 在插槽间尽可能多的转移指定数量的物品
        ///// </summary>
        ///// <param name="_sender"></param>
        ///// <param name="_receiver"></param>
        ///// <param name="_transferAmount"></param>
        ///// <param name="_lostType"></param>
        ///// <param name="_sourceType"></param>
        //public static void TransferItemWithMinCountBetweenSlot(BaseInventorySlot _sender, BaseInventorySlot _receiver,
        //                                                       uint _transferAmount, string _lostType,
        //                                                       string _sourceType)
        //{
        //    if (_sender.ItemInfo.ItemID != _receiver.ItemInfo.ItemID)
        //        return;

        //    _transferAmount = (uint)Mathf.Min(_transferAmount,
        //                                      (int)_sender.ItemAmount,
        //                                      (int)_receiver.RemainAmountInSlot);

        //    _sender.Remove(_transferAmount, _lostType);
        //    _receiver.Add(_transferAmount, _sourceType);
        //}

        ///// <summary>
        ///// 在插槽间尽可能多的转移物品
        ///// </summary>
        ///// <param name="_sender"></param>
        ///// <param name="_receiver"></param>
        ///// <param name="_lostType"></param>
        ///// <param name="_sourceType"></param>
        //public static void TransferItemWithMinCountBetweenSlot(BaseInventorySlot _sender, BaseInventorySlot _receiver,
        //                                                       string _lostType=null, string _sourceType = null)
        //{
        //    if (_sender.ItemInfo.ItemID != _receiver.ItemInfo.ItemID)
        //        return;

        //    uint transferAmount = (uint)Mathf.Min((int)_sender.ItemAmount,
        //                                          (int)_receiver.RemainAmountInSlot);

        //    _sender.Remove(transferAmount, _lostType);
        //    _receiver.Add(transferAmount, _sourceType);
        //}

        ///// <summary>
        ///// 转移插槽
        ///// </summary>
        ///// <param name="_sender"></param>
        ///// <param name="_receiver"></param>
        ///// <param name="_slot"></param>
        ///// <param name="_lostType"></param>
        ///// <param name="_sourceType"></param>
        ///// <returns></returns>
        //public static bool TransferSlot(BaseInventory _sender, BaseInventory _receiver, BaseInventorySlot _slot, 
        //                                string _lostType = null, string _sourceType = null)
        //{
        //    //TODO
        //    return true;
        //}

        #endregion
    }
}