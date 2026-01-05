using System;
using System.Collections.Generic;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Models
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        [HideInInspector]
        private List<SerializableKeyValuePair<TKey, TValue>> m_SerializedDataList = new List<SerializableKeyValuePair<TKey, TValue>>();

        public void OnBeforeSerialize()
        {
            //在序列化之前调用，将字典数据转换为列表
            m_SerializedDataList.Clear();

            foreach (var kv in this)
            {
                m_SerializedDataList.Add(new SerializableKeyValuePair<TKey, TValue>(kv.Key, kv.Value));
            }
        }

        public void OnAfterDeserialize()
        {
            //在反序列化之后调用，将列表数据还原为字典
            this.Clear();

            foreach (var kv in m_SerializedDataList)
            {
                this[kv.m_Key] = kv.m_Value;
            }
        }
    }

    [Serializable]
    public struct SerializableKeyValuePair<TKey, TValue>
    {
        public TKey m_Key;
        public TValue m_Value;

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            m_Key = key;
            m_Value = value;
        }
    }
}