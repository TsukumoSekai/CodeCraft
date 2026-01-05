using System.Collections.Generic;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility { 
public static class DictionaryUtility
{
        #region 拓展方法

        /// <summary>
        /// 如果不存在Key，则添加
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="_dictionary"></param>
        /// <param name="_key"></param>
        /// <param name="_value"></param>
        public static bool TryAdd<K, V>(this Dictionary<K, V> _dictionary, K _key, V _value)
        {
            if (!_dictionary.ContainsKey(_key))
            {
                _dictionary.Add(_key, _value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 如果存在Key，则移除
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="_dictionary"></param>
        /// <param name="_key"></param>
        public static bool TryRemove<K, V>(this Dictionary<K, V> _dictionary, K _key)
        {
            if (_dictionary.ContainsKey(_key))
            {
                _dictionary.Remove(_key);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 如果存在Key则替换Value，否则添加新KV
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="_dictionary"></param>
        /// <param name="_key"></param>
        /// <param name="_value"></param>
        public static void AddOrReplace<K, V>(this Dictionary<K, V> _dictionary, K _key, V _value)
        {
            if (_dictionary.ContainsKey(_key))
                _dictionary[_key] = _value;
            else
                _dictionary.Add(_key, _value);
        }

        /// <summary>
        /// 如果存在Key则返回对应Value，否则返回默认值
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="_dictionary"></param>
        /// <param name="_key"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool TryGetValue<K, V>(this Dictionary<K, V> _dictionary, K _key, out V _value)
        {
            if (!_dictionary.TryGetValue(_key, out _value))
                return true;
            else
                _value = default;
            return false;
        }

        /// <summary>
        /// 获取字典中随机键值
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="_dictionary"></param>
        /// <returns></returns>
        public static KeyValuePair<K, V> GetRandomKV<K, V>(this Dictionary<K, V> _dictionary)
        {
            int random = Random.Range(0, _dictionary.Count);
            int index = 0;

            foreach (KeyValuePair<K, V> kv in _dictionary)
            {
                if (index == random)
                    return kv;
                index++;
            }

            return default;
        }

        #endregion
    }
}