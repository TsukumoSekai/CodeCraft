#if PACKAGE_NEWTONSOFT_JSON
using System;
using OfflineFantasy.GameCraft.ItemContainer;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Item
{
    public interface IItemInfo { }

    /// <summary>
    /// 背包物品接口
    /// </summary>
    public interface IItemInfo<TKey, TItemType> : IItemInfo
    {
        /// <summary>
        /// 物品数据ID
        /// </summary>
        public TKey ItemDataID { get; }

        /// <summary>
        /// 所属物品栏插槽
        /// </summary>
        public BaseInventorySlot<TKey, TItemType> InventorySlot { get; }

        /// <summary>
        /// 物品名称
        /// </summary>
        public string ItemName { get; }

        /// <summary>
        /// 物品描述
        /// </summary>
        public string ItemDescription { get; }

        /// <summary>
        /// 物品图标路径
        /// </summary>
        public Sprite ItemIcon { get; }

        /// <summary>
        /// 物品数量
        /// </summary>
        public uint Amount { get; }

        /// <summary>
        /// 最大堆叠数量
        /// </summary>
        public uint MaxAmountInSlot { get; }

        /// <summary>
        /// 获取物品的时间
        /// </summary>
        public DateTime GetTime { get; }

        [Newtonsoft.Json.JsonIgnore]
        public IItemData<TKey, TItemType> ItemData { get; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="_amount"></param>
        /// <param name="_sourceType"></param>
        /// <returns></returns>
        public IItemInfo<TKey, TItemType> Add(uint _amount, string _sourceType);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="_amount"></param>
        /// <param name="_lostType"></param>
        /// <returns></returns>
        public bool Remove(uint _amount, string _lostType);

        public void Initialize(BaseInventorySlot<TKey, TItemType> _slot);
    }

    //public class InventoryItemConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type _objectType)
    //    {
    //        Debug.Log(_objectType.Name);
    //        return _objectType is IItemInfo;
    //    }

    //    public override object ReadJson(JsonReader _reader, Type _objectType, object _existingValue, JsonSerializer _serializer)
    //    {
    //        if (_reader.TokenType == JsonToken.StartObject)
    //        {
    //            return _serializer.Deserialize(_reader, _objectType);

    //            //JObject jObj = JObject.Load(_reader);

    //            //IInventoryItem item = InventoryUtility.CreateInventoryItem((uint)jObj[nameof(IInventoryItem.ItemID)], (uint)jObj[nameof(IInventoryItem.Amount)]);

    //            //switch (item.ItemData.m_ItemType)
    //            //{
    //            //    case Utility.Enums.ItemType.None:
    //            //        break;
    //            //    case Utility.Enums.ItemType.Equipment:
    //            //        break;
    //            //    case Utility.Enums.ItemType.Material:
    //            //        break;
    //            //    case Utility.Enums.ItemType.Consumables:
    //            //        break;
    //            //    case Utility.Enums.ItemType.Tool:
    //            //        break;
    //            //}

    //            //return item;
    //        }

    //        throw new JsonException($"Unexpected token type: {_reader.TokenType}");
    //    }

    //    public override void WriteJson(JsonWriter _writer, object _value, JsonSerializer _serializer)
    //    {
    //        if (_value != null)
    //        {
    //            JObject jObj = new JObject(_value);

    //            jObj.WriteTo(_writer);
    //        }
    //        else
    //            JValue.CreateNull().WriteTo(_writer);
    //    }
    //}
}
#endif