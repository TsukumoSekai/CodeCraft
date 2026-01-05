using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static OfflineFantasy.GameCraft.Utility.ComparerUtility;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class RandomUtility
    {
        public static bool RandomValue(float _value, ComparerType _comparer, bool _log = false, string _content = "获取随机浮点数结果")
        {
            float randomValue = Random.value;

            bool result = Compare(_value, randomValue, _comparer);

            //if (_log)
            //Debug.Log($"{_content},  比较值:  {_value},  样本值:  {randomValue},  结果:  {result},  取样范围:  {0}~{1}");

            return result;
        }

        /// <summary>
        /// 获取随机浮点数结果
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_comparer"></param>
        /// <param name="_content"></param>
        /// <param name="_minInclusive"></param>
        /// <param name="_maxInclusive"></param>
        /// <returns></returns>
        public static bool RandomRange(float _value, ComparerType _comparer, float _minInclusive = 1f, float _maxInclusive = 100f, bool _log = false, string _content = "获取随机浮点数结果")
        {
            float randomValue = Random.Range(_minInclusive, _maxInclusive);

            bool result = Compare(_value, randomValue, _comparer);

            //if (_log)
            //Debug.Log($"{_content},  比较值:  {_value},  样本值:  {randomValue},  结果:  {result},  取样范围:  {_minInclusive}~{_maxInclusive}");

            return result;
        }

        /// <summary>
        /// 获取随机整数结果（随机值不包括_rangeMax）
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_comparer"></param>
        /// <param name="_content"></param>
        /// <param name="_minInclusive"></param>
        /// <param name="_maxExclusive"></param>
        /// <returns></returns>
        public static bool RandomRange(int _value, ComparerType _comparer, int _minInclusive = 0, int _maxExclusive = 100, bool _log = false, string _content = "获取随机整数结果")
        {
            int randomValue = Random.Range(_minInclusive, _maxExclusive);

            bool result = Compare(_value, randomValue, _comparer);

            //if (_log)
            //Debug.Log($"{_content},  比较值:  {_value},  样本值:  {randomValue},  结果:  {result},  取样范围:  {_minInclusive}~{_maxExclusive}");

            return result;
        }

        /// <summary>
        /// 获取随机权重结果
        /// </summary>
        /// <param name="_randomWeightArray"></param>
        /// <param name="_minInclusive"></param>
        /// <param name="_maxInclusive"></param>
        /// <returns></returns>
        public static int RandomWeight(IList<float> _randomWeightArray, float _minInclusive, float _maxInclusive, bool _log = false, string _content = "获取随机权重结果")
        {
            float randomValue = Random.Range(_minInclusive, _maxInclusive);
            float weight = _minInclusive;

            int cycleTimes = _randomWeightArray.Count - 1;

            for (int i = 0; i < cycleTimes; i++)
            {
                weight += _randomWeightArray[i];

                if (randomValue < weight)
                {
                    //if (_log)
                    //Debug.Log($"{_content},  索引:  {i},  样本值:  {randomValue},  取样范围:  {_minInclusive}~{_maxInclusive},  区间:  {weight - _randomWeightArray[i]}~{weight}");

                    return i;
                }
            }

            //if (_log)
            //Debug.Log($"{_content},  样本值:  {randomValue},  索引:  {_randomWeightArray.Count - 1},  取样范围:  {_minInclusive}~{_maxInclusive},  命中区间:  {weight}~{_maxInclusive}");

            return _randomWeightArray.Count - 1;
        }

        public static int RandomWeight(IList<int> _randomWeightArray, int _minInclusive = 0, int _maxExclusive = 0, bool _log = false, string _content = "获取随机权重结果")
        {
            if (_maxExclusive == 0)
            {
                foreach (var weight in _randomWeightArray)
                {
                    _maxExclusive += weight;
                }
            }

            int randomValue = Random.Range(_minInclusive, _maxExclusive);
            int intervalLimit = _minInclusive;

            int cycleTimes = _randomWeightArray.Count - 1;

            for (int i = 0; i < cycleTimes; i++)
            {
                intervalLimit += _randomWeightArray[i];

                //5 5 5
                //0-4  5-9  10-14

                if (randomValue < intervalLimit)
                {
                    //if (_log)
                    //Debug.Log($"{_content},  索引:  {i},  样本值:  {randomValue},  取样范围:  {_minInclusive}~{_maxInclusive},  区间:  {weight - _randomWeightArray[i]}~{weight}");

                    return i;
                }
            }

            return _randomWeightArray.Count - 1;
        }

        public static int RandomWeight(IList<float> _randomWeightArray, bool _log = false, string _content = "获取随机权重结果")
        {
            float sum = 0f;

            foreach (float value in _randomWeightArray)
            {
                sum += value;
            }

            float randomValue = Random.Range(0, sum);
            float upperLimit = 0f;
            float lowerLimit = 0f;

            for (int i = 0; i < _randomWeightArray.Count - 1; i++)
            {
                lowerLimit = upperLimit;
                upperLimit += _randomWeightArray[i];

                if (randomValue <= upperLimit)
                {
                    //if (_log)
                    ////Debug.Log($"{_content},  索引:  {i},  样本值:  {randomValue},  取样范围:  {0}~{sum},  样本区间:  {weight - _randomWeightArray[i]}~{weight}");
                    //Debug.Log($"{_content},  索引:  {i},  样本值:  {randomValue},  取样范围:  {0}~{sum},  样本区间:  {lowerLimit}~{upperLimit}");

                    return i;
                }
            }

            //if (_log)
            //Debug.Log($"{_content},  样本值:  {randomValue},  索引:  {_randomWeightArray.Count - 1},  取样范围:  {0}~{sum},  样本区间:  {upperLimit}~{sum}");

            return _randomWeightArray.Count - 1;
        }

        /// <summary>
        /// 获取随机布尔结果
        /// </summary>
        /// <param name="_content"></param>
        /// <returns></returns>
        public static bool RandomBool(bool _log = false, string _content = "获取随机布尔结果")
        {
            bool result = Random.Range(0, 2) == 1;

            //if (_log)
            //Debug.Log($"{_content},  结果:  {result}");

            return result;
        }

        /// <summary>
        /// 获取随机值
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static float GetRandomValue(this Vector2 _value)
        {
            if (_value.x < _value.y)
                return Random.Range(_value.x, _value.y);
            else if (_value.x > _value.y)
                return Random.Range(_value.y, _value.x);
            else
                return _value.x;
        }

        /// <summary>
        /// 获取随机值
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static int GetRandomValue(this Vector2Int _value)
        {
            if (_value.x < _value.y)
                return Random.Range(_value.x, _value.y + 1);
            else if (_value.x > _value.y)
                return Random.Range(_value.y, _value.x + 1);
            else
                return _value.x;
        }

        #region 列表

        /// <summary>
        /// 获取随机元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_list"></param>
        /// <param name="_minInclusive"></param>
        /// <param name="_maxExclusive"></param>
        /// <param name="_removeElement"></param>
        /// <returns></returns>
        public static T GetRandomElement<T>(this IList<T> _list, int _minInclusive = 0, int _maxExclusive = -1, bool _removeElement = false)
        {
            //return _list[Random.Range(0, _list.Count)];

            if (_list == null)
                return default;

            if (_list.Count == 0)
                return default;

            if (_maxExclusive < _list.Count)
                _maxExclusive = _list.Count;

            int index = Random.Range(_minInclusive, _maxExclusive);
            var result = _list[index];

            if (_removeElement)
                _list.RemoveAt(index);

            return result;
        }

        public static bool TryGetRandomElement<T>(this IList<T> _list, out T _element, int _minInclusive = 0, int _maxExclusive = -1, bool _removeElement = false)
        {
            if (_list == null)
            {
                _element = default;
                return false;
            }

            int length = _list.Count;

            if (length == 0)
            {
                _element = default;
                return false;
            }

            _element = _list.GetRandomElement(_minInclusive, _maxExclusive, _removeElement);
            return true;
        }

        /// <summary>
        /// 获取多个随机元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_list"></param>
        /// <param name="_count"></param>
        /// <returns></returns>
        public static T[] GetMultipleRandomElementValue<T>(this IList<T> _list, int _count)
        {
            if (_count >= _list.Count)
                return _list.ToArray();

            List<T> sourceList = new List<T>(_list);
            T[] result = new T[_count];
            T element;

            for (int i = 0; i < _count; i++)
            {
                element = sourceList.GetRandomElement();
                sourceList.Remove(element);
                result[i] = element;
            }

            return result;
        }

        #endregion
    }
}