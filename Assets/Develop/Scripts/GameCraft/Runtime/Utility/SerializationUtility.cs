using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfflineFantasy.GameCraft.Utility.Interface;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class SerializationUtility
    {
        public static readonly char[] FieldSeparator = new char[] { ',' };

        /// <summary>
        /// 分隔符
        /// </summary>
        public static readonly char[] SeparatorArray = new char[]
        {
            ';',
            ':',
            '*',
            '|',
            '$',
        };

        /// <summary>
        /// 解析变量值
        /// </summary>
        /// <param name="_variableType"></param>
        /// <param name="_variableString"></param>
        /// <param name="_variableObject"></param>
        /// <returns></returns>
        public static bool ParseVariable(Type _variableType, string _variableString, out object _variableObject)
        {
            try
            {
                if (_variableType == typeof(string))
                    _variableObject = _variableString;

                else if (_variableType == typeof(float))
                    _variableObject = float.Parse(_variableString);

                else if (_variableType == typeof(int))
                    _variableObject = int.Parse(_variableString);

                else if (_variableType == typeof(uint))
                    _variableObject = uint.Parse(_variableString);

                else if (_variableType.IsEnum)
                    _variableObject = int.Parse(_variableString);

                else if (_variableType == typeof(bool))
                    _variableObject = _variableString.Trim() != "0";

                else if (_variableType == typeof(sbyte))
                    _variableObject = sbyte.Parse(_variableString);

                else if (_variableType == typeof(byte))
                    _variableObject = byte.Parse(_variableString);

                else if (_variableType == typeof(short))
                    _variableObject = short.Parse(_variableString);

                else if (_variableType == typeof(ushort))
                    _variableObject = ushort.Parse(_variableString);

                else if (_variableType == typeof(long))
                    _variableObject = long.Parse(_variableString);

                else if (_variableType == typeof(ulong))
                    _variableObject = ulong.Parse(_variableString);

                else
                {
                    DebugCraft.LogError("未找到该数值的对应类型 :  " + _variableType);
                    _variableObject = null;
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                DebugCraft.LogError($"解析数值类型错误:  {_variableType.ToString()},  {_variableString}");
                throw e;
            }
        }

        #region 特定类型

        /// <summary>
        /// 序列化二维向量
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_separatorIndex"></param>
        /// <returns></returns>
        public static string SerializeVector2(Vector2 _value, int _separatorIndex = 0)
        {
            return $"{_value.x}{SeparatorArray[_separatorIndex]}{_value.y}";
        }

        /// <summary>
        /// 反序列化二维向量
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_separatorIndex"></param>
        /// <returns></returns>
        public static Vector2 DeserializeVector2(string _value, int _separatorIndex = 0)
        {
            string[] str = _value.Split(SeparatorArray[_separatorIndex]);
            return new Vector2(float.Parse(str[0]), float.Parse(str[1]));
        }

        /// <summary>
        /// 序列化三维向量
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_separatorIndex"></param>
        /// <returns></returns>
        public static string SerializeVector3(Vector3 _value, int _separatorIndex = 0)
        {
            return $"{_value.x}{SeparatorArray[_separatorIndex]}{_value.y}{SeparatorArray[_separatorIndex]}{_value.z}";
        }

        /// <summary>
        /// 反序列化三维向量
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_separatorIndex"></param>
        /// <returns></returns>
        public static Vector3 DeserializeVector3(string _value, int _separatorIndex = 0)
        {
            string[] str = _value.Split(SeparatorArray[_separatorIndex]);
            return new Vector3(float.Parse(str[0]), float.Parse(str[1]), float.Parse(str[2]));
        }

        /// <summary>
        /// 序列化二维整数向量
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_separatorIndex"></param>
        /// <returns></returns>
        public static string SerializeVector2Int(Vector2Int _value, int _separatorIndex = 0)
        {
            return $"{_value.x}{SeparatorArray[_separatorIndex]}{_value.y}";
        }

        /// <summary>
        /// 反序列化二维整数向量
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_separatorIndex"></param>
        /// <returns></returns>
        public static Vector2Int DeserializeVector2Int(string _value, int _separatorIndex = 0)
        {
            string[] str = _value.Split(SeparatorArray[_separatorIndex]);
            return new Vector2Int(float.Parse(str[0]).RoundToInt(), float.Parse(str[1]).RoundToInt());
        }

        /// <summary>
        /// 序列化三维整数向量
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_separatorIndex"></param>
        /// <returns></returns>
        public static string SerializeVector3Int(Vector3Int _value, int _separatorIndex = 0)
        {
            return $"{_value.x}{SeparatorArray[_separatorIndex]}{_value.y}{SeparatorArray[_separatorIndex]}{_value.z}";
        }

        /// <summary>
        /// 反序列化三维整数向量
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_separatorIndex"></param>
        /// <returns></returns>
        public static Vector3Int DeserializeVector3Int(string _value, int _separatorIndex = 0)
        {
            string[] str = _value.Split(SeparatorArray[_separatorIndex]);
            return new Vector3Int(float.Parse(str[0]).RoundToInt(), float.Parse(str[1]).RoundToInt(), float.Parse(str[2]).RoundToInt());
        }

        /// <summary>
        /// 序列化颜色
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_separatorIndex"></param>
        /// <returns></returns>
        public static string SerializeColor(Color _value, int _separatorIndex = 0)
        {
            return $"{_value.r}{SeparatorArray[_separatorIndex]}{_value.g}{SeparatorArray[_separatorIndex]}{_value.b}{SeparatorArray[_separatorIndex]}{_value.a}";
        }

        /// <summary>
        /// 反序列化颜色
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_separatorIndex"></param>
        /// <returns></returns>
        public static Color DeserializeColor(string _value, int _separatorIndex = 0)
        {
            string[] str = _value.Split(SeparatorArray[_separatorIndex]);
            return str.Length == 3 ?
                   new Color(float.Parse(str[0]), float.Parse(str[1]), float.Parse(str[2]), 1f) :
                   new Color(float.Parse(str[0]), float.Parse(str[1]), float.Parse(str[2]), float.Parse(str[3]));
        }

        /// <summary>
        /// 反序列化枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_value"></param>
        /// <param name="_separatorIndex"></param>
        /// <returns></returns>
        public static T DeserializeEnum<T>(string _value, int _separatorIndex = 0)
        {
            return (T)(object)(int.Parse(_value));
        }

        #endregion

        #region 泛型

        /// <summary>
        /// 序列化常见类型物体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_objects"></param>
        /// <param name="_separator"></param>
        /// <returns></returns>
        public static string SerializeCommonObjects<T>(IList<T> _objects, int _separatorIndex = 0)
        {
            StringBuilder sb = new StringBuilder();
            char separator = SeparatorArray[_separatorIndex];

            for (int i = 0; i < _objects.Count; i++)
            {
                if (i > 0)
                    sb.Append(separator);

                sb.Append(_objects[i].ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// 反序列化常见类型物体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_context"></param>
        /// <param name="_separatorIndex"></param>
        /// <returns></returns>
        public static T[] DeserializeCommonObjects<T>(string _context, int _separatorIndex = 0)
        {
            if (string.IsNullOrEmpty(_context))
                return new T[0];

            string[] contextArray = _context.Split(SeparatorArray[_separatorIndex++]);

            Type type = typeof(T);

            object[] objects = new object[contextArray.Length];
            string element;

            for (int i = 0; i < contextArray.Length; i++)
            {
                element = contextArray[i];

                if (type == typeof(string))
                    objects[i] = element;
                else if (type == typeof(uint))
                    objects[i] = uint.TryParse(element, out uint uintValue) ? uintValue : 0;
                else if (type == typeof(int))
                    objects[i] = int.TryParse(element, out int intValue) ? intValue : 0;
                else if (type == typeof(float))
                    objects[i] = float.TryParse(element, out float floatValue) ? floatValue : 0f;
                else if (type == typeof(Vector2))
                    objects[i] = DeserializeVector2(element, _separatorIndex);
                else if (type == typeof(Vector3))
                    objects[i] = DeserializeVector3(element, _separatorIndex);
                else if (type == typeof(Vector2Int))
                    objects[i] = DeserializeVector2Int(element, _separatorIndex);
                else if (type == typeof(Vector3Int))
                    objects[i] = DeserializeVector3Int(element, _separatorIndex);
                else if (type == typeof(Color))
                    objects[i] = DeserializeColor(element, _separatorIndex);
                else if (type.IsEnum)
                    objects[i] = DeserializeEnum<T>(element, _separatorIndex);
                else
                    DebugCraft.LogError($"转换类型错误:  {typeof(T).FullName}");
            }

            return objects.Cast<T>().ToArray();
        }

        /// <summary>
        /// 序列化可序列化物体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_objects"></param>
        /// <param name="_splitSymbolIndex"></param>
        /// <returns></returns>
        public static string SerializeSerializableObjects<T>(IList<T> _objects, int _splitSymbolIndex = 0) where T : ISerializable<T>, new()
        {
            StringBuilder sb = new StringBuilder();
            int nextSplitSymbolIndex = _splitSymbolIndex + 1;

            foreach (ISerializable<T> @object in _objects)
            {
                if (sb.Length > 0)
                    sb.Append(SeparatorArray[_splitSymbolIndex]);

                sb.Append(@object.Serialize(nextSplitSymbolIndex));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 反序列化可序列化物体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_context"></param>
        /// <param name="_splitSymbolIndex"></param>
        /// <returns></returns>
        public static T[] DeserializeSerializableObjects<T>(string _context, int _splitSymbolIndex = 0) where T : ISerializable<T>, new()
        {
            try
            {

                string[] contextArray = _context.Split(SeparatorArray[_splitSymbolIndex]);
                int nextSplitSymbolIndex = _splitSymbolIndex + 1;

                List<T> list = new List<T>();
                //T @object;

                for (int i = 0; i < contextArray.Length; i++)
                {
                    if (new T().Deserialize(contextArray[i], out T @object, nextSplitSymbolIndex))
                        list.Add(@object);
                }

                //return System.Array.ConvertAll(contextArray, context => new T().Deserialize(context, nextSplitSymbolIndex));
                return list.ToArray();
            }
            catch (Exception e)
            {
                Debug.LogError($"反序列化字符串格式错误:  {_context}");
                throw e;
            }
        }

        #endregion
    }
}