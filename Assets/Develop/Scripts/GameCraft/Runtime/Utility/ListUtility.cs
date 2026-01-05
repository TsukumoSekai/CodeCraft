using System.Collections.Generic;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class ListUtility
{
        #region 拓展方法

        public static void AddAtOrReplace<T>(this List<T> _list, int _index, T _value)
        {
            while (_list.Count < _index + 1)
            {
                _list.Add(default);
            }
            _list[_index] = _value;
        }

        public static T TryGetValue<T>(this List<T> _list, int _index)
        {
            if (_list.Count > _index && _list[_index] != null)
                return _list[_index];
            else
                return default;
        }

        /// <summary>
        /// 如果不包含相同元素则添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_list"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool TryAdd<T>(this List<T> _list, T _value)
        {
            if (!_list.Contains(_value))
            {
                _list.Add(_value);
                return true;
            }
            return false;
        }

        public static void TryAddRange<T>(this List<T> _list, IList<T> _valueList)
        {
            foreach (T value in _valueList)
            {
                _list.TryAdd(value);
            }
        }

        public static void RemoveLast<T>(this List<T> _list)
        {
            _list.RemoveAt(_list.Count - 1);
        }

        public static int RemoveAllSpecifiedlElement<T>(this List<T> _list, T _value)
        {
            int removedCount = 0;

            do
            {
                if (_list.Remove(_value))
                    removedCount++;
                else
                    return removedCount;
            }
            while (true);
        }

        public static void RemoveRange<T>(this List<T> _list, List<T> _removeList)
        {
            foreach (T value in _removeList)
            {
                _list.Remove(value);
            }
        }

        ///// <summary>
        ///// 获取多个随机元素
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="_list"></param>
        ///// <param name="_count"></param>
        ///// <returns></returns>
        //public static List<T> GetMultipleRandomElementValue<T>(this List<T> _list, int _count)
        //{
        //    if (_count >= _list.Count)
        //        return new List<T>(_list);

        //    //List<T> sourceList = _list.DeepClone();
        //    List<T> sourceList = new List<T>();
        //    List<T> result = new List<T>();
        //    T element;

        //    for (int i = 0; i < _count; i++)
        //    {
        //        element = sourceList.GetRandomElement();
        //        sourceList.Remove(element);
        //        result.Add(element);
        //    }

        //    return result;
        //}

        /// <summary>
        /// 交换元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_list"></param>
        /// <param name="_index_0"></param>
        /// <param name="_index_1"></param>
        public static void Swap<T>(this List<T> _list, int _index_0, int _index_1)
        {
            if (_index_0 == _index_1 || _index_0 >= _list.Count || _index_1 >= _list.Count)
                return;

            T temp = _list[_index_0];
            _list[_index_0] = _list[_index_1];
            _list[_index_1] = temp;
        }

        /// <summary>
        /// 移动元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_list"></param>
        /// <param name="_sourceIndex"></param>
        /// <param name="_newIndex"></param>
        public static void Move<T>(this List<T> _list, int _sourceIndex, int _newIndex)
        {
            if (_sourceIndex == _newIndex || _sourceIndex >= _list.Count || _newIndex >= _list.Count)
                return;

            T temp = _list[_sourceIndex];
            _list.RemoveAt(_sourceIndex);

            //if (_sourceIndex < _newIndex)
            //    _newIndex--;

            _list.Insert(_newIndex, temp);
        }

        /// <summary>
        /// 打乱列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_list"></param>
        /// <param name="_shuffleCount"></param>
        public static void Shuffle<T>(this List<T> _list, int _shuffleCount = 1)
        {
            if (_list.Count <= 1)
                return;

            int index;

            for (int i = 0; i < _shuffleCount; i++)
            {
                for (int j = 0; j < _list.Count; j++)
                {
                    do
                    {
                        index = Random.Range(0, _list.Count);
                    } while (j == index);

                    _list.Swap(j, index);
                }
            }
        }

        #endregion
    }
}