using System;
using System.Linq;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class ArrayUtility
    {
        public static void Add<T>(ref T[] _array, T _item)
        {
            Array.Resize(ref _array, _array.Length + 1);
            _array[_array.Length - 1] = _item;

            //List<T> list = _array.ToList();
            //list.Add(_item);
            //_array = list.ToArray();
        }

        public static bool Remove<T>(ref T[] _array, T _item)
        {
            return RemoveAt(ref _array, Array.IndexOf(_array, _item));

            int i = Array.IndexOf(_array, _item);

            if (i < 0)
                return false;

            int length = _array.Length - 1;

            for (; i < length; i++)
            {
                _array[i] = _array[i + 1];
            }

            //List<T> list = _array.ToList();

            //if (list.Remove(_item))
            //{
            //    _array = list.ToArray();
            //    return true;
            //}

            return false;
        }

        public static bool RemoveAt<T>(ref T[] _array, int _index)
        {
            if (_index < 0 || _index >= _array.Length)
                return false;

            int length = _array.Length - 1;

            for (; _index < length; _index++)
            {
                _array[_index] = _array[_index + 1];
            }

            //List<T> list = _array.ToList();
            //list.RemoveAt(_index);
            //_array = list.ToArray();

            return true;
        }

        #region 拓展方法

        /// <summary>
        /// 交换元素索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_array"></param>
        /// <param name="_srcIndex"></param>
        /// <param name="_dstIndex"></param>
        public static void Swap<T>(this T[] _array, int _srcIndex, int _dstIndex)
        {
            T temp = _array[_srcIndex];
            _array[_srcIndex] = _array[_dstIndex];
            _array[_dstIndex] = temp;
        }

        /// <summary>
        /// 移除重复元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_array"></param>
        public static T[] RemoveDuplicateElements<T>(this T[] _array)
        {
            return _array.GroupBy(e => e).Select(e => e.Key).ToArray();
        }

        #endregion
    }
}