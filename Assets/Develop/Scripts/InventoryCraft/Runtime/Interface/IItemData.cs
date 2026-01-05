#if PACKAGE_NEWTONSOFT_JSON
using UnityEngine;

namespace OfflineFantasy.GameCraft.Item
{
    public interface IItemData<TKey, TItemType>
    {
        /// <summary>
        /// 物品ID
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public TKey ItemDataID { get; }

        /// <summary>
        /// 物品类型
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public TItemType ItemType { get; }

        /// <summary>
        /// 物品名称
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string ItemName { get; }

        /// <summary>
        /// 物品描述
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string ItemDescription { get; }

        /// <summary>
        /// 物品图标
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public Sprite ItemIcon { get; }
    }
}
#endif