using System;
using System.Collections.Generic;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class EnumUtility
    {
        #region 拓展方法

        //TODO：待测试

        public static bool HasFlag(this Enum _enum, Enum _flags)
        {
            ulong enumValue = Convert.ToUInt64(_enum);
            ulong flagValue = Convert.ToUInt64(_flags);

            return (enumValue & flagValue) == flagValue;
        }

        public static Enum AddFlags(this Enum _enum, Enum _flags)
        {
            ulong enumValue = Convert.ToUInt64(_enum);
            ulong flagValue = Convert.ToUInt64(_flags);

            return (Enum)Enum.ToObject(_enum.GetType(), enumValue | flagValue);
        }

        public static Enum RemoveFlags(this Enum _enum, Enum _flags)
        {
            ulong enumValue = Convert.ToUInt64(_enum);
            ulong flagValue = Convert.ToUInt64(_flags);

            return (Enum)Enum.ToObject(_enum.GetType(), enumValue & ~flagValue);
        }

        public static IEnumerable<Enum> GetFlags(this Enum _enum)
        {
            foreach (Enum flag in Enum.GetValues(_enum.GetType()))
            {
                if (_enum.HasFlag(flag))
                    yield return flag;
            }
        }

        //public static string ToLocalization(this Enum _enum)
        //{
        //    I18nAttribute attribute = _enum.GetEnumAttribute<I18nAttribute>();

        //    return attribute != null ?
        //           attribute.ToString() :
        //           _enum.ToString();
        //}

        //public static string GetPath(this Enum _enum)
        //{
        //    PathAttribute attribute = _enum.GetEnumAttribute<PathAttribute>();

        //    return attribute != null ?
        //           attribute.ToString() :
        //           _enum.ToString();
        //}

        //public static T GetEnumAttribute<T>(this Enum _enum) where T : class
        //{
        //    Type type = _enum.GetType();
        //    string key = string.Format("{0} {1}", type.ToString(), _enum.ToString());

        //    using (DictionaryPool<string, FieldInfo>.Get(out var fieldInfoDict))
        //    {
        //        if (!fieldInfoDict.TryGetValue(key, out FieldInfo fieldInfo))
        //        {
        //            fieldInfo = type.GetField(_enum.ToString());
        //            fieldInfoDict.Add(key, fieldInfo);
        //        }

        //        return Attribute.GetCustomAttribute(fieldInfo, typeof(T)) as T;
        //    }
        //}

        #endregion
    }
}