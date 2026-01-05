using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility { 
public static class DoubleUtility
{
        #region 拓展方法

        /// <summary>
        /// 四舍六入五成双
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static int RoundToInt(this double _value)
        {
            return Mathf.RoundToInt((float)_value);
        }

        /// <summary>
        /// 向上取整
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static int CeilToInt(this double _value)
        {
            return Mathf.CeilToInt((float)_value);
        }

        /// <summary>
        /// 向下取整
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static int FloorToInt(this double _value)
        {
            return Mathf.FloorToInt((float)_value);
        }

        /// <summary>
        /// 添加正负号
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static string AddSymbol(this double _value)
        {
            string symbol = _value >= 0 ? "+" : "-";
            return $"{symbol}{_value.ToString()}";
        }

        #endregion
    }
}