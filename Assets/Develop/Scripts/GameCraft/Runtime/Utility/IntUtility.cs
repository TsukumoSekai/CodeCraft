using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class IntUtility
    {
        /// <summary>
        /// 获取指定范围中两数值间长度
        /// </summary>
        /// <param name="_isPositiveOrder"></param>
        /// <param name="_value_From"></param>
        /// <param name="_value_To"></param>
        /// <param name="_minInclusive"></param>
        /// <param name="_maxInclusive"></param>
        /// <returns></returns>
        public static int GetLength(bool _isPositiveOrder, int _value_From, int _value_To, int _minInclusive, int _maxInclusive)
        {
            if (_isPositiveOrder)
            {
                if (_value_From <= _value_To)
                    return _value_To - _value_From;
                else
                    return _maxInclusive - _value_From + _minInclusive + _value_To;
            }
            else
            {
                if (_value_From <= _value_To)
                    return _maxInclusive - _value_To + _minInclusive + _value_From;
                else
                    return _value_From - _value_To;
            }

        }

        /// <summary>
        /// 使数值在指定范围内增减
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_isIncrease"></param>
        /// <param name="_minInclusive"></param>
        /// <param name="_maxExclusive"></param>
        /// <returns></returns>
        public static int IncreaseInRange(int _value, bool _isIncrease, int _minInclusive, int _maxExclusive)
        {
            int added = _isIncrease ? 1 : -1;
            return IncreaseInRange(_value, added, _minInclusive, _maxExclusive);
        }

        /// <summary>
        /// 使数值在指定范围内增减
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_added"></param>
        /// <param name="_minInclusive"></param>
        /// <param name="_maxExclusive"></param>
        /// <returns></returns>
        public static int IncreaseInRange(int _value, int _added, int _minInclusive, int _maxExclusive)
        {
            if (_added == 0)
                return _value;

            int result = _value;
            int symbol = _added > 0 ? 1 : -1;

            for (int i = 0; i < Mathf.Abs(_added); i++)
            {
                result += symbol;

                if (result >= _maxExclusive)
                    result = _minInclusive;
                else if (result < _minInclusive)
                    result = _maxExclusive - 1;
            }

            return result;
        }

        public static int TryParse(string _value)
        {
            return int.TryParse(_value, out int result) ?
                   result :
                   -1;
        }
    }
}