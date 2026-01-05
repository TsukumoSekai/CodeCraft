using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class FloatUtility
    {
        /// <summary>
        /// 重映射
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_sourceMin">原区间最小值</param>
        /// <param name="_sourceMax">原区间最大值</param>
        /// <param name="_targetMin">目标区间最小值</param>
        /// <param name="_targetMax">目标区间最大值</param>
        /// <returns></returns>
        public static float Remap(float _value, float _sourceMin, float _sourceMax, float _targetMin, float _targetMax)
        {
            return (_value - _sourceMin) / (_sourceMax - _sourceMin) * (_targetMax - _targetMin) + _targetMin;
        }

        /// <summary>
        /// 获取幂的和
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_power"></param>
        /// <returns></returns>
        public static float GetPowerSum(float _value, float _power)
        {
            return _power > 0 ?
                   GetPowerSum(_value, _power - 1f) + Mathf.Pow(2f, _power) :
                   Mathf.Pow(2f, _power);
        }

        public static bool CheckValueInRange(float _value, float _min, float _max, bool _includeBoundary = false)
        {
            return _includeBoundary ?
                   _value >= _min && _value <= _max :
                   _value > _min && _value < _max;
        }

        /// <summary>
        /// 除法运算
        /// </summary>
        /// <param name="_dividend">被除数</param>
        /// <param name="_divisor">除数</param>
        /// <param name="_quotient">商</param>
        /// <param name="_remainder">余数</param>
        /// <returns>被除数能否被整除</returns>
        public static bool Division(float _dividend, float _divisor, out float _quotient, out float _remainder)
        {
            _quotient = _dividend / _divisor;
            _remainder = _dividend % _divisor;

            return _remainder == 0;
        }

        #region 拓展方法

        /// <summary>
        /// 四舍六入五成双
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static int RoundToInt(this float _value)
        {
            return Mathf.RoundToInt(_value);
        }

        /// <summary>
        /// 向上取整
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static int CeilToInt(this float _value)
        {
            return Mathf.CeilToInt(_value);
        }

        /// <summary>
        /// 向下取整
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static int FloorToInt(this float _value)
        {
            return Mathf.FloorToInt(_value);
        }

        /// <summary>
        /// 添加正负号
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static string AddSymbol(this float _value)
        {
            string symbol = _value >= 0 ? "+" : "-";
            return $"{symbol}{_value.ToString()}";
        }

        /// <summary>
        /// 测距结果类型
        /// </summary>
        public enum RangingResultType
        {
            GreaterThanRange,
            LessThanRange,
            InRange,
        }

        /// <summary>
        /// 测距
        /// </summary>
        /// <param name="_distance"></param>
        /// <param name="_range"></param>
        /// <returns></returns>
        public static RangingResultType Ranging(this float _distance, Vector2 _range)
        {
            if (_distance > _range.y)
                return RangingResultType.GreaterThanRange;
            else if (_distance < _range.x)
                return RangingResultType.LessThanRange;
            else
                return RangingResultType.InRange;
        }

        #endregion
    }
}