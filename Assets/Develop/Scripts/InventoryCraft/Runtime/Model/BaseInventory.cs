using System;
using System.Collections.Generic;
using OfflineFantasy.GameCraft.Item;
using OfflineFantasy.GameCraft.Utility.Event;
using UnityEngine;

namespace OfflineFantasy.GameCraft.ItemContainer
{
    /// <summary>
    /// 背包基类
    /// </summary>
    public abstract class BaseInventory<TKey, TItemType>
    {
        #region 保护字段

        [Newtonsoft.Json.JsonProperty(Required = Newtonsoft.Json.Required.Default)]
        protected uint m_TotalSlotCount;

        [Newtonsoft.Json.JsonProperty(Required = Newtonsoft.Json.Required.Default)]
        protected List<BaseInventorySlot<TKey, TItemType>> m_SlotList = new List<BaseInventorySlot<TKey, TItemType>>();

        //protected List<BaseInventorySlot> m_SlotBufferList = new List<BaseInventorySlot>();

        #endregion

        #region 公共字段

        /// <summary>
        /// 默认移除空插槽
        /// </summary>
        public bool m_DefaultRemoveEmptySlot = true;

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

        /// <summary>
        /// 创建插槽事件
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public Action<BaseInventorySlot<TKey, TItemType>> m_OnCreateSlotEvent;

        /// <summary>
        /// 销毁插槽事件
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public Action<BaseInventorySlot<TKey, TItemType>> m_OnDestroySlotEvent;

        #endregion

        #region 属性

        [Newtonsoft.Json.JsonIgnore]
        public abstract IItemInfoFactory<TKey, TItemType> ItemInfoFactory { get; }

        /// <summary>
        /// 背包总槽位数量
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public virtual uint TotalSlotCount => m_TotalSlotCount;

        /// <summary>
        /// 背包剩余空槽位数量
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public uint RemainSlotCount => (uint)(TotalSlotCount - m_SlotList.Count);

        /// <summary>
        /// 背包名称
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public abstract string InventoryName { get; }

        /// <summary>
        /// 背包插槽列表
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public virtual List<BaseInventorySlot<TKey, TItemType>> SlotList => m_SlotList;

        #endregion

        #region 构造方法

        public BaseInventory() { }

        public BaseInventory(uint _totalSlotCount)
        {
            m_TotalSlotCount = _totalSlotCount;
        }

        #endregion

        #region 保护方法

        /// <summary>
        /// 判断背包内是否有空余容量
        /// </summary>
        /// <param name="_itemID"></param>
        /// <param name="_amount"></param>
        /// <param name="_remainSlotBufferList"></param>
        /// <param name="_remainSlotCapacity"></param>
        /// <param name="_emptySlotCapacity"></param>
        /// <returns></returns>
        protected virtual bool CheckHasEmptyCapacity(TKey _itemID, uint _amount, out List<BaseInventorySlot<TKey, TItemType>> _remainSlotBufferList, out uint _remainSlotCapacity, out uint _emptySlotCapacity)
        {
            GetCanAddItemSlotListAndRemainAmount(_itemID, out _remainSlotBufferList, out _remainSlotCapacity);
            _emptySlotCapacity = GetCanAddAmountInAllEmptySlot(_itemID);

            return _remainSlotCapacity >= _amount ||
                   _emptySlotCapacity >= _amount ||
                   _remainSlotCapacity + _emptySlotCapacity >= _amount;
        }

        /// <summary>
        /// 获取背包内可添加指定ID物品的槽位及剩余可堆叠数量
        /// </summary>
        /// <param name="_itemID"></param>
        /// <param name="_slotBufferList"></param>
        /// <param name="_remainAmount"></param>
        protected virtual void GetCanAddItemSlotListAndRemainAmount(TKey _itemID, out List<BaseInventorySlot<TKey, TItemType>> _slotBufferList, out uint _remainAmount)
        {
            _slotBufferList = GetSlotList(_itemID);
            _remainAmount = 0;
            BaseInventorySlot<TKey, TItemType> slot;

            for (int i = _slotBufferList.Count - 1; i >= 0; i--)
            {
                slot = _slotBufferList[i];
                _remainAmount += slot.RemainAmountInSlot;
                if (slot.ItemAmount == slot.MaxAmountInSlot)
                    _slotBufferList.RemoveAt(i);
            }
        }

        /// <summary>
        /// 获取指定ID物品在背包内单个空槽位可堆叠数量
        /// </summary>
        /// <param name="_itemID"></param>
        /// <returns></returns>
        protected virtual uint GetCanAddAmountInSingleEmptySlot(TKey _itemID)
        {
            return InventoryUtility.GetMaxAmountInSlotByID(_itemID);
            //return (uint)Mathf.Min(InventoryUtility.GetMaxAmountInSlotByID(_itemID), m_InventorySlot.MaxAmountInSlot);
        }

        /// <summary>
        /// 获取指定ID物品在背包内所有空槽位可堆叠数量
        /// </summary>
        /// <param name="_itemID"></param>
        /// <returns></returns>
        protected virtual uint GetCanAddAmountInAllEmptySlot(TKey _itemID)
        {
            return RemainSlotCount * GetCanAddAmountInSingleEmptySlot(_itemID);
        }

        /// <summary>
        /// 添加物品到已有槽位
        /// </summary>
        /// <param name="_itemID"></param>
        /// <param name="_amount"></param>
        /// <param name="_slotBufferList"></param>
        /// <returns></returns>
        protected virtual bool AddItemToExistingSlot(uint _amount, string _sourceType, List<BaseInventorySlot<TKey, TItemType>> _slotBufferList)
        {
            if (_slotBufferList == null)
            {
                Debug.LogError($"槽位列表为空,  {InventoryName}");

                return false;
            }

            BaseInventorySlot<TKey, TItemType> slot;
            uint remainAmount;

            for (int i = 0; i < _slotBufferList.Count; i++)
            {
                slot = _slotBufferList[i];

                if (_amount > slot.RemainAmountInSlot)
                {
                    remainAmount = slot.RemainAmountInSlot;
                    slot.Add(remainAmount, _sourceType);
                    _amount -= remainAmount;
                }
                else if (_amount > 0)
                {
                    slot.Add(_amount, _sourceType);
                }
            }

            return true;
        }

        /// <summary>
        /// 添加物品到空槽位
        /// </summary>
        /// <param name="_itemID"></param>
        /// <param name="_amount"></param>
        /// <returns></returns>
        protected virtual bool AddItemToEmptySlot(TKey _itemID, TItemType _itemType, uint _amount, string _sourceType)
        {
            uint canAddAmountInEmptySlot = GetCanAddAmountInAllEmptySlot(_itemID);

            if (canAddAmountInEmptySlot < _amount)
            {
                Debug.LogWarning($"无法添加物品，背包内剩余容量不足,  {InventoryName}");

                return false;
            }

            IItemInfo<TKey, TItemType> itemInfo;
            BaseInventorySlot<TKey, TItemType> slot;
            uint maxAmountInSlot = GetCanAddAmountInSingleEmptySlot(_itemID);

            while (_amount > maxAmountInSlot)
            {
                itemInfo = ItemInfoFactory.GenerateIItemInfo(_itemID, _itemType, maxAmountInSlot) as IItemInfo<TKey, TItemType>;
                slot = BaseInventorySlot<TKey, TItemType>.Create(itemInfo, _sourceType);
                slot.Initialize(this);

                m_SlotList.Add(slot);

                _amount -= maxAmountInSlot;

                m_OnCreateSlotEvent?.Invoke(slot);
                EventCenter<InventoryEventType>.Broadcast(InventoryEventType.CreatItemSlot, slot);

                OnGetItem(_itemID, maxAmountInSlot, slot, _sourceType);
            }

            itemInfo = ItemInfoFactory.GenerateIItemInfo(_itemID, _itemType, _amount) as IItemInfo<TKey, TItemType>;
            slot = BaseInventorySlot<TKey, TItemType>.Create(itemInfo, _sourceType);
            slot.Initialize(this);

            m_SlotList.Add(slot);

            m_OnCreateSlotEvent?.Invoke(slot);
            EventCenter<InventoryEventType>.Broadcast(InventoryEventType.CreatItemSlot, slot);

            OnGetItem(_itemID, _amount, slot, _sourceType);

            return true;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize()
        {
            foreach (BaseInventorySlot<TKey, TItemType> slot in m_SlotList)
            {
                slot.Initialize(this);
            }
        }

        #region 添加物品

        /// <summary>
        /// 添加物品
        /// </summary>
        /// <param name="_itemID"></param>
        /// <param name="_amount"></param>
        /// <returns></returns>
        public virtual bool AddItem(TKey _itemID, TItemType _itemType, uint _amount, string _sourceType)
        {
            if (!CheckHasEmptyCapacity(_itemID, _amount, out List<BaseInventorySlot<TKey, TItemType>> slotBufferList, out uint canAddAmountInRemainSlot, out _))
            {
                Debug.LogWarning($"背包已满:  {InventoryName},  无法放入 {_amount} 个 {_itemID}");
                return false;
            }

            //Debug.Log($"{InventoryName} 被放入了 {_amount} 个 {_itemID}");

            if (canAddAmountInRemainSlot >= _amount)
            {
                AddItemToExistingSlot(_amount, _sourceType, slotBufferList);
            }
            else
            {
                AddItemToExistingSlot(canAddAmountInRemainSlot, _sourceType, slotBufferList);
                AddItemToEmptySlot(_itemID, _itemType, _amount - canAddAmountInRemainSlot, _sourceType);
            }

            return true;
        }

        /// <summary>
        /// 添加物品
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_amount"></param>
        /// <param name="_modifiedSlotList"></param>
        /// <param name="_sourceType"></param>
        /// <returns></returns>
        public virtual bool AddItem(IItemInfo<TKey, TItemType> _item, uint _amount, string _sourceType)
        {
            return AddItem(_item.ItemDataID, _item.ItemData.ItemType, _amount, _sourceType);
        }

        /// <summary>
        /// 添加物品
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_modifiedSlotList"></param>
        /// <param name="_sourceType"></param>
        /// <returns></returns>
        public virtual bool AddItem(IItemInfo<TKey, TItemType> _item, string _sourceType)
        {
            return AddItem(_item.ItemDataID, _item.ItemData.ItemType, _item.Amount, _sourceType);
        }

        /// <summary>
        /// 向指定插槽添加物品
        /// </summary>
        /// <param name="_slot"></param>
        /// <param name="_amount"></param>
        /// <param name="_sourceType"></param>
        /// <returns></returns>
        public virtual bool AddItemToSpecified(BaseInventorySlot<TKey, TItemType> _slot, uint _amount, string _sourceType)
        {
            if (!m_SlotList.Contains(_slot) || _slot.RemainAmountInSlot < _amount)
                return false;

            //Debug.Log($"{InventoryName} 被放入了 {_amount} 个 {_slot.ItemInfo.ItemDataID}");

            _slot.Add(_amount, _sourceType);

            return true;
        }

        /// <summary>
        /// 添加插槽
        /// </summary>
        /// <param name="_slot"></param>
        /// <param name="_sourceType"></param>
        /// <returns></returns>
        public virtual bool AddSlot(BaseInventorySlot<TKey, TItemType> _slot, string _sourceType, bool _ignoreEmpty = true)
        {
            if (RemainSlotCount == 0 || _slot == null || _slot.ItemInfo == null || (_slot.ItemAmount == 0 && _ignoreEmpty))
                return false;

            //Debug.Log($"{InventoryName} 被放入了 {_slot.ItemAmount} 个 {_slot.ItemInfo.ItemDataID}");

            _slot.Initialize(this);

            m_SlotList.Add(_slot);

            m_OnCreateSlotEvent?.Invoke(_slot);
            EventCenter<InventoryEventType>.Broadcast(InventoryEventType.CreatItemSlot, _slot);

            OnGetItem(_slot.ItemInfo.ItemDataID, _slot.ItemAmount, _slot, _sourceType);

            return true;
        }

        #endregion

        #region 移除物品

        /// <summary>
        /// 移除物品
        /// </summary>
        /// <param name="_itemID"></param>
        /// <param name="_removeAmount"></param>
        /// <param name="_deleteEmptySlot"></param>
        /// <returns></returns>
        public virtual bool RemoveItem(TKey _itemID, uint _removeAmount, string _lostType = null, bool _deleteEmptySlot = true)
        {
            if (GetSpecifiedItemCount(_itemID) < _removeAmount)
                return false;

            //Debug.Log($"{InventoryName} 被移除了 {_removeAmount} 个 {_itemID}");

            List<BaseInventorySlot<TKey, TItemType>> slotList = GetSlotList(_itemID);
            BaseInventorySlot<TKey, TItemType> slot;

            for (int i = slotList.Count - 1; i >= 0; i--)
            {
                slot = slotList[i];

                if (slot.ItemAmount <= _removeAmount)
                {
                    _removeAmount -= slot.ItemAmount;

                    slot.Remove(slot.ItemAmount, _lostType);

                    if (_removeAmount == 0)
                        break;
                }
                else
                {
                    slot.Remove(_removeAmount, _lostType);
                    break;
                }
            }

            return true;
        }

        /// <summary>
        /// 移除物品
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_amount"></param>
        /// <param name="_modifiedSlotList"></param>
        /// <param name="_deleteEmptySlot"></param>
        /// <returns></returns>
        public virtual bool RemoveItem(IItemInfo<TKey, TItemType> _item, uint _amount, string _lostType, bool _deleteEmptySlot = true)
        {
            return RemoveItem(_item.ItemDataID, _amount, _lostType, _deleteEmptySlot);
        }

        /// <summary>
        /// 移除物品
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_modifiedSlotList"></param>
        /// <param name="_deleteEmptySlot"></param>
        /// <returns></returns>
        public virtual bool RemoveItem(IItemInfo<TKey, TItemType> _item, string _lostType)
        {
            return RemoveItem(_item.ItemDataID, _item.Amount, _lostType);
        }

        /// <summary>
        /// 移除所有指定ID的物品
        /// </summary>
        /// <param name="_itemID"></param>
        /// <param name="_modifiedSlotList"></param>
        /// <param name="_deleteEmptySlot"></param>
        public virtual void RemoveAllSpecifiedItem(TKey _itemID, string _lostType)
        {
            RemoveItem(_itemID, GetSpecifiedItemCount(_itemID), _lostType);
        }

        public virtual void RemoveAllItem(string _lostType)
        {
            for (int i = m_SlotList.Count - 1; i >= 0; i--)
            {
                RemoveItem(m_SlotList[i].ItemInfo, _lostType);
            }
        }

        /// <summary>
        /// 从指定插槽移除物品
        /// </summary>
        /// <param name="_slot"></param>
        /// <param name="_amount"></param>
        /// <returns></returns>
        public virtual bool RemoveItemFromSpecifiedSlot(BaseInventorySlot<TKey, TItemType> _slot, uint _amount, string _lostType)
        {
            if (!m_SlotList.Contains(_slot) || _slot.ItemAmount < _amount)
                return false;

            //Debug.Log($"{InventoryName} 被移除了 {_amount} 个 {_slot.ItemInfo.ItemDataID}");

            _slot.Remove(_amount, _lostType);

            return true;
        }

        /// <summary>
        /// 移除指定插槽
        /// </summary>
        /// <param name="_slot"></param>
        /// <returns></returns>
        public virtual bool RemoveSlot(BaseInventorySlot<TKey, TItemType> _slot, string _lostType)
        {
            if (m_SlotList.Remove(_slot))
            {
                //Debug.Log($"{InventoryName} 被移除了 {_slot.ItemAmount} 个 {_slot.ItemInfo.ItemDataID}");

                m_OnLostItemEvent?.Invoke(new LoseItemEventInfo<TKey, TItemType>(_lostType, _slot.ItemInfo.ItemDataID, _slot.ItemInfo.Amount, _slot, this));
                m_OnDestroySlotEvent?.Invoke(_slot);
                EventCenter<InventoryEventType>.Broadcast(InventoryEventType.DestroyItemSlot, _slot);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 分割物品
        /// </summary>
        /// <param name="_itemID"></param>
        /// <param name="_amount"></param>
        /// <param name="_item"></param>
        /// <returns></returns>
        public virtual bool SplitItem(TKey _itemID, uint _amount, out IItemInfo<TKey, TItemType> _item, bool _deleteEmptySlot = true)
        {
            //TODO:待完善
            Debug.LogError($"待完善");
            _item = null;
            return false;
            //_item = null;

            //if (!RemoveItem(_itemID, _amount, GameResourcesLostType.Internal, _deleteEmptySlot))
            //    return false;

            //_item = InventoryUtility.CreateInventoryItem(_itemID, _amount);

            //return true;
        }

        #endregion

        #region 获取信息

        /// <summary>
        /// 获取背包内指定ID物品可以放入的数量
        /// </summary>
        /// <param name="_itemID"></param>
        /// <returns></returns>
        public uint GetCanAddItemAmount(TKey _itemID)
        {
            GetCanAddItemSlotListAndRemainAmount(_itemID, out _, out uint canAddAmountInRemainSlot);
            uint canAddAmountInEmptySlot = GetCanAddAmountInAllEmptySlot(_itemID);

            return canAddAmountInRemainSlot + canAddAmountInEmptySlot;
        }

        /// <summary>
        /// 获取背包内指定物品数量
        /// </summary>
        /// <param name="_item"></param>
        /// <returns></returns>
        public uint GetSpecifiedItemCount(IItemInfo<TKey, TItemType> _item)
        {
            return GetSpecifiedItemCount(_item.ItemDataID);
        }

        /// <summary>
        /// 获取背包内指定ID物品的数量
        /// </summary>
        /// <param name="_itemID"></param>
        /// <returns></returns>
        public uint GetSpecifiedItemCount(TKey _itemID)
        {
            uint amount = 0;
            List<BaseInventorySlot<TKey, TItemType>> slotList = GetSlotList(_itemID);

            if (slotList.Count == 0)
                return amount;

            foreach (BaseInventorySlot<TKey, TItemType> slot in slotList)
            {
                amount += slot.ItemAmount;
            }

            return amount;
        }

        /// <summary>
        /// 获取背包内所有物品数量总和
        /// </summary>
        /// <returns></returns>
        public uint GetAllItemCount()
        {
            uint amount = 0;

            foreach (BaseInventorySlot<TKey, TItemType> slot in SlotList)
            {
                amount += slot.ItemAmount;
            }

            return amount;
        }

        /// <summary>
        /// 获取背包内所有包含指定ID物品的槽位
        /// </summary>
        /// <param name="_itemID"></param>
        /// <param name="slotBufferList"></param>
        public List<BaseInventorySlot<TKey, TItemType>> GetSlotList(TKey _itemID)
        {
            return m_SlotList.FindAll(slot => slot.ItemInfo.ItemDataID.Equals(_itemID));
        }

        ///// <summary>
        ///// 获取背包内所有包含指定类型及指定品质物品的槽位
        ///// </summary>
        ///// <param name="_itemType"></param>
        ///// <returns></returns>
        //public List<BaseInventorySlot> GetSlotList(ItemType _itemType)
        //{
        //    if (_itemType == ItemType.None)
        //        return m_InventorySlotList;
        //    else
        //        return m_InventorySlotList.FindAll(slot => slot.ItemInfo.ItemData.m_ItemType.HasAnyFlags(_itemType));
        //}

        public List<BaseInventorySlot<TKey, TItemType>> GetSlotList(Predicate<BaseInventorySlot<TKey, TItemType>> _condition)
        {
            return m_SlotList.FindAll(_condition);
        }

        #endregion

        #region 判断

        /// <summary>
        /// 判断指定插槽是否属于该背包
        /// </summary>
        /// <param name="_slot"></param>
        /// <returns></returns>
        public bool CheckSlotBelongToInventory(BaseInventorySlot<TKey, TItemType> _slot)
        {
            return m_SlotList.Contains(_slot);
        }

        /// <summary>
        /// 判断是否用足够容量
        /// </summary>
        /// <param name="_itemID"></param>
        /// <param name="_amount"></param>
        /// <returns></returns>
        public bool CheckHasEmptyCapacity(TKey _itemID, uint _amount)
        {
            return CheckHasEmptyCapacity(_itemID, _amount, out _, out _, out _);
        }

        /// <summary>
        /// 判断背包内是否有指定物品
        /// </summary>
        /// <param name="_itemID"></param>
        /// <returns></returns>
        public bool CheckHasItem(TKey _itemID)
        {
            BaseInventorySlot<TKey, TItemType> slot = m_SlotList.Find(slot => slot.ItemInfo.ItemDataID.Equals(_itemID));

            return slot != null && slot.ItemAmount > 0;
        }

        /// <summary>
        /// 判断背包内是否有指定物品及指定数量
        /// </summary>
        /// <param name="_itemID"></param>
        /// <param name="_itemAmount"></param>
        /// <returns></returns>
        public bool CheckHasItem(TKey _itemID, uint _itemAmount)
        {
            uint currentAmount = 0;

            foreach (BaseInventorySlot<TKey, TItemType> slot in m_SlotList)
            {
                if (slot.ItemInfo.ItemDataID.Equals(_itemID))
                {
                    currentAmount += slot.ItemAmount;

                    if (currentAmount >= _itemAmount)
                        return true;
                }
            }

            return false;
        }

        #endregion

        ///// <summary>
        ///// 排序
        ///// </summary>
        ///// <param name="_comparer"></param>
        //public void Sort(IComparer<BaseInventorySlot> _comparer, bool _reverse = false)
        //{
        //    m_InventorySlotList.Sort(_comparer);

        //    if (_reverse)
        //        m_InventorySlotList.Reverse();
        //}

        /// <summary>
        /// 倒序
        /// </summary>
        public void Reverse()
        {
            m_SlotList.Reverse();
        }

        /// <summary>
        /// 获得物品时
        /// </summary>
        /// <param name="_itemID"></param>
        /// <param name="_itemAmount"></param>
        /// <param name="_slot"></param>
        /// <param name="_sourceType"></param>
        public void OnGetItem(TKey _itemID, uint _itemAmount, BaseInventorySlot<TKey, TItemType> _slot, string _sourceType)
        {
            GetItemEventInfo<TKey, TItemType> getItemEventInfo = new GetItemEventInfo<TKey, TItemType>(_sourceType, _itemID, _itemAmount, _slot, this);
            m_OnGetItemEvent?.Invoke(getItemEventInfo);
            EventCenter<InventoryEventType>.Broadcast(InventoryEventType.GetItem, getItemEventInfo);
        }

        //TODO:考虑将BaseInventorySlot做为BaseInventory的子类，因为OnGetItem方法不应该能被外部调用，或者Inventory在创建插槽时监听其获得和丢失？
        public void OnGetItem(GetItemEventInfo<TKey, TItemType> _getItemEventInfo)
        {
            m_OnGetItemEvent?.Invoke(_getItemEventInfo);
            EventCenter<InventoryEventType>.Broadcast(InventoryEventType.GetItem, _getItemEventInfo);
        }

        /// <summary>
        /// 失去物品时
        /// </summary>
        /// <param name="_itemID"></param>
        /// <param name="_itemAmount"></param>
        /// <param name="_slot"></param>
        /// <param name="_lostType"></param>
        public void OnLoseItem(TKey _itemID, uint _itemAmount, BaseInventorySlot<TKey, TItemType> _slot, string _lostType)
        {
            m_OnLostItemEvent?.Invoke(new LoseItemEventInfo<TKey, TItemType>(_lostType, _itemID, _itemAmount, _slot, this));

            if (_slot.ItemAmount == 0 && m_DefaultRemoveEmptySlot)
            {
                m_OnDestroySlotEvent?.Invoke(_slot);

                m_SlotList.Remove(_slot);
                EventCenter<InventoryEventType>.Broadcast(InventoryEventType.DestroyItemSlot, _slot);
            }
        }

        public void OnLoseItem(LoseItemEventInfo<TKey, TItemType> _loseItemEventInfo)
        {
            m_OnLostItemEvent?.Invoke(_loseItemEventInfo);
        }

        #endregion
    }
}