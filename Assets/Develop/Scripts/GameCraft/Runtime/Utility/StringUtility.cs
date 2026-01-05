using System;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class StringUtility
    {
        #region 拓展方法

        /// <summary>
        /// 解析为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static T ParseToEnum<T>(this string _value)
        {
            return (T)Enum.Parse(typeof(T), _value);
        }

        #endregion
    }
}