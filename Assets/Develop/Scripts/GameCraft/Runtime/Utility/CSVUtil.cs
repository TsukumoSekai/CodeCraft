using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public class CSVFileHelper
    {
        /// <summary>
        /// 将DataTable中数据写入到CSV文件中
        /// </summary>
        /// <param name="dt">提供保存数据的DataTable</param>
        /// <param name="fileName">CSV的文件路径</param>
        public static void SaveCSV(DataTable dt, string fullPath)
        {
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            string data = "";
            //写出列名称
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);
            //写出各行数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string str = dt.Rows[i][j].ToString();
                    str = str.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号
                    if (str.Contains(',') || str.Contains('"')
                        || str.Contains('\r') || str.Contains('\n')) //含逗号 冒号 换行符的需要放到引号中
                    {
                        str = string.Format("\"{0}\"", str);
                    }

                    data += str;
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CSVElementAttribute : Attribute
    {
        public string key { get; set; }
        public string defaultVal { get; set; }

        //#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
        //        public string referClassName { get; set; }
        //#endif
        public CSVElementAttribute(string key, string defaultVal = null)
        {
            this.key = key;
            this.defaultVal = defaultVal;
            //#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
            //            this.referClassName = referClass;
            //#endif
        }
    }

    public static class CSVUtil
    {
        public const string SYMBOL_COMMENT = "#";

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
        public static string[] SYMBOL_LINE = new string[] { Environment.NewLine };//行分隔符
#else
        public static string[] SYMBOL_LINE = new string[] { "\r\n" };//打包csv配表统一使用CRLF
#endif

        public static char[] SYMBOL_FIELD = new char[] { ',' };//字段分割符
        public static char[] SYMBOL_FIRST = new char[] { ';' };//字段分割符
        public static char[] SYMBOL_SECOND = new char[] { ':' };//字段分割符
        public static char[] SYMBOL_THIRD = new char[] { '*' };//字段分割符
        public static char[] SYMBOL_FOURTH = new char[] { '|' };
        public static char[] SYMBOL_FIVE = new char[] { '$' };
        public static char[] SYMBOL_NULL = new char[] { ' ' };

        public static uint[] GetArrayByString(string str)
        {
            return GetArrayByString(str, SYMBOL_FIRST);
        }

        public static uint[] GetArrayByString(string str, char[] symbol)
        {
            string[] strArr = str.Split(symbol, StringSplitOptions.RemoveEmptyEntries);
            int len = strArr.Length;
            uint[] result = new uint[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = uint.Parse(strArr[i]);
            }
            return result;
        }

        public static string[] GetStrArrayByString(string str, char[] symbol)
        {
            string[] strArr = str.Split(symbol, StringSplitOptions.RemoveEmptyEntries);
            int len = strArr.Length;
            string[] result = new string[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = strArr[i];
            }
            return result;
        }

        public static int[] GetIntArrayByString(string str)
        {
            return GetIntArrayByString(str, SYMBOL_FIRST);
        }

        private static int[] GetIntArrayByString(string str, char[] symbol)
        {
            string[] strArr = str.Split(symbol, StringSplitOptions.RemoveEmptyEntries);
            int len = strArr.Length;
            int[] result = new int[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = int.Parse(strArr[i]);
            }
            return result;
        }

        public static float[] GetFloatArrayByString(string str)
        {
            return GetFloatArrayByString(str, SYMBOL_FIRST);
        }

        private static float[] GetFloatArrayByString(string str, char[] symbol)
        {
            string[] strArr = str.Split(symbol, StringSplitOptions.RemoveEmptyEntries);
            int len = strArr.Length;
            float[] result = new float[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = float.Parse(strArr[i]);
            }
            return result;
        }

        private static Dictionary<string, int> ParseTitle(string csvTitleString)
        {
            csvTitleString.Trim();
            Dictionary<string, int> result = new Dictionary<string, int>();
            string[] arr = csvTitleString.Split(SYMBOL_FIELD);
            for (int i = 0, len = arr.Length; i < len; i++)
            {
                result.Add(arr[i], i);
            }
            return result;
        }

        public static List<KeyValuePair<TKey, TValue>> GetArrayByString<TKey, TValue>(string str)
            where TValue : struct
            where TKey : struct
        {
            string[] strArr = str.Split(SYMBOL_FIRST, StringSplitOptions.RemoveEmptyEntries);
            int len = strArr.Length;
            List<KeyValuePair<TKey, TValue>> list = new List<KeyValuePair<TKey, TValue>>();
            string[] tempArr = null;
            Type keyType = typeof(TKey), valueType = typeof(TValue);
            System.Object keyObject, valueObject;
            for (int i = 0; i < len; i++)
            {
                tempArr = strArr[i].Split(SYMBOL_SECOND);
                if (keyType == typeof(string))
                    keyObject = tempArr[0];
                else
                    keyObject = Convert.ChangeType(tempArr[0], keyType);
                if (valueType == typeof(string))
                    valueObject = tempArr[1];
                else
                    valueObject = Convert.ChangeType(tempArr[1], valueType);
                KeyValuePair<TKey, TValue> kvp = new KeyValuePair<TKey, TValue>((TKey)keyObject, (TValue)valueObject);
                list.Add(kvp);
            }
            return list;
        }


        private static void ParseValue(Type type, string keyName, ref System.Object valueObject, string valueString, string csvPath, int line = 0)
        {
            try
            {
                if (type == typeof(sbyte))
                {
                    valueObject = sbyte.Parse(valueString);
                }
                else if (type == typeof(byte))
                {
                    valueObject = byte.Parse(valueString);
                }
                else if (type == typeof(short))
                {
                    valueObject = short.Parse(valueString);
                }
                else if (type == typeof(ushort))
                {
                    valueObject = ushort.Parse(valueString);
                }
                else if (type == typeof(int))
                {
                    valueObject = int.Parse(valueString);
                }
                else if (type == typeof(uint))
                {
                    valueObject = uint.Parse(valueString);
                }
                else if (type == typeof(long))
                {
                    valueObject = long.Parse(valueString);
                }
                else if (type == typeof(ulong))
                {
                    valueObject = ulong.Parse(valueString);
                }
                else if (type == typeof(float))
                {
                    valueObject = float.Parse(valueString);
                }

                else if (type.IsEnum)
                {
                    valueObject = int.Parse(valueString);
                }
                else if (type == typeof(bool))
                {
                    valueObject = valueString.Trim() != "0";
                }
                else if (type == typeof(string))
                {
                    valueObject = valueString;
                }
                else
                {
                    throw new Exception("CSVUtil_ERROR: have not process type" + type);
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("CSVUtil_ERROR({0}): {1} (key name :{2}, error value:{3},line:{4})", csvPath, e.Message, keyName, valueString, line + 1));
            }
        }

        /// <summary>
        /// 解析为dic
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="csvPath"></param>
        /// <param name="keyCSVMark"></param>
        /// <returns></returns>
        public static Dictionary<K, T> Parse<K, T>(string csvPath, string keyCSVMark) where T : new()
        {
            TextAsset ta = Resources.Load<TextAsset>(csvPath);

            if (ta == null)
            {
                Debug.LogError($"CSV文件不存在:  {csvPath}");
                return null;
            }

            Dictionary<K, T> result = new Dictionary<K, T>();

            string[] lineArr = ta.text.Split(SYMBOL_LINE, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, int> titleDic = ParseTitle(lineArr[0]);

            for (int i = 1; i < lineArr.Length; i++)//第一行是title所以i=1开始
            {
                K key = default(K);
                string[] strArr = lineArr[i].Trim().Split(SYMBOL_FIELD, StringSplitOptions.None);

                T instance = System.Activator.CreateInstance<T>();
                FieldInfo[] fieldArr = instance.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                for (int k = 0, fieldLen = fieldArr.Length; k < fieldLen; k++)
                {
                    FieldInfo field = fieldArr[k];
                    System.Object[] attrArr = field.GetCustomAttributes(typeof(CSVElementAttribute), false);
                    if (attrArr.Length > 0)
                    {
                        CSVElementAttribute csvele = attrArr[0] as CSVElementAttribute;
                        string valueString = string.Empty;
                        if (titleDic.ContainsKey(csvele.key))
                        {
                            valueString = strArr[titleDic[csvele.key]];
                        }
                        else if (csvele.defaultVal != null)
                        {
                            valueString = csvele.defaultVal;
                        }
                        else
                        {
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
                            DebugCraft.LogError(string.Format("csv({0}) column not found : {1}, or define its default value in code", csvPath, csvele.key));
#endif
                        }
                        System.Object valueObject = null;
                        ParseValue(field.FieldType, csvele.key, ref valueObject, valueString, csvPath, i);

                        field.SetValue(instance, valueObject);

                        if (csvele.key == keyCSVMark)
                        {
                            key = (K)valueObject;
                        }
                    }
                }

                PropertyInfo[] propertyInfoArr = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                for (int k = 0, propertyLen = propertyInfoArr.Length; k < propertyLen; k++)
                {
                    PropertyInfo pi = propertyInfoArr[k];
                    System.Object[] attrArr = pi.GetCustomAttributes(typeof(CSVElementAttribute), false);
                    if (attrArr.Length > 0)
                    {
                        CSVElementAttribute csvele = attrArr[0] as CSVElementAttribute;

                        string valueString = string.Empty;
                        if (titleDic.ContainsKey(csvele.key))
                        {
                            valueString = strArr[titleDic[csvele.key]];
                        }
                        else if (csvele.defaultVal != null)
                        {
                            valueString = csvele.defaultVal;
                        }
                        else
                        {
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
                            DebugCraft.LogError(string.Format("csv({0}) column not found : {1}, or define its default value in code", csvPath, csvele.key));
#endif
                        }

                        System.Object valueObject = null;
                        ParseValue(pi.PropertyType, csvele.key, ref valueObject, valueString, csvPath, i);
                        pi.SetValue(instance, valueObject, null);

                        if (csvele.key == keyCSVMark)
                        {
                            key = (K)valueObject;
                        }
                    }
                }
                result.Add(key, instance);
            }

            return result;
        }
        /// <summary>
        /// 解析为单类，见GameData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csvPath"></param>
        /// <returns></returns>
        public static T ParseClass<T>(string csvPath) where T : new()
        {
            TextAsset ta = null;
            string taText = null;
            string dataPath = Application.streamingAssetsPath;
            var filePath = dataPath + "/" + csvPath + ".csv";
            if (File.Exists(filePath))
            {
                taText = File.ReadAllText(filePath);
            }
            else
            {
                ta = Resources.Load<TextAsset>(csvPath);

                if (ta != null)
                    taText = ta.text;
            }
            if (taText == null)
                return default(T);
            T instance = System.Activator.CreateInstance<T>();
            FieldInfo[] fieldArr = instance.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            string[] lineArr = taText.Split(CSVUtil.SYMBOL_LINE, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> lineDic = new Dictionary<string, string>();
            for (int i = 0, len = lineArr.Length; i < len; i++)
            {
                string line = lineArr[i].Trim();
                if (line.StartsWith(SYMBOL_COMMENT))
                    continue;
                string[] strArr = line.Split(CSVUtil.SYMBOL_FIELD, 2);
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
                if (lineDic.ContainsKey(strArr[0]))
                {
                    DebugCraft.LogError(string.Format("csv({0}) column already exist: {1}", csvPath, strArr[0]));
                }
#endif
                lineDic.Add(strArr[0], strArr[1]);
            }

            for (int k = 0; k < fieldArr.Length; k++)
            {
                FieldInfo field = fieldArr[k];
                System.Object[] attrArr = field.GetCustomAttributes(typeof(CSVElementAttribute), false);
                if (attrArr.Length > 0)
                {
                    CSVElementAttribute csvele = attrArr[0] as CSVElementAttribute;
                    string valueString = string.Empty;
                    if (lineDic.ContainsKey(csvele.key))
                    {
                        valueString = lineDic[csvele.key];
                    }
                    else if (csvele.defaultVal != null)
                    {
                        valueString = csvele.defaultVal;
                    }
                    else
                    {
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
                        DebugCraft.LogError(string.Format("csv({0}) column not found : {1}, or define its default value in code", csvPath, csvele.key));
#endif
                    }

                    System.Object valueObject = null;
                    ParseValue(field.FieldType, csvele.key, ref valueObject, valueString, csvPath);

                    field.SetValue(instance, valueObject);

                }
            }

            PropertyInfo[] propertyInfoArr = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int k = 0, propertyLen = propertyInfoArr.Length; k < propertyLen; k++)
            {
                PropertyInfo pi = propertyInfoArr[k];
                System.Object[] attrArr = pi.GetCustomAttributes(typeof(CSVElementAttribute), false);
                if (attrArr.Length > 0)
                {
                    CSVElementAttribute csvele = attrArr[0] as CSVElementAttribute;

                    string valueString = string.Empty;
                    if (lineDic.ContainsKey(csvele.key))
                    {
                        valueString = lineDic[csvele.key];
                    }
                    else if (csvele.defaultVal != null)
                    {
                        valueString = csvele.defaultVal;
                    }
                    else
                    {
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
                        throw new Exception(string.Format("CSVUtil_ERROR({0}): key not found (key name :{1})", csvPath, csvele.key));
#endif
                    }
                    System.Object valueObject = null;
                    ParseValue(pi.PropertyType, csvele.key, ref valueObject, valueString, csvPath);
                    pi.SetValue(instance, valueObject, null);

                }
            }
            return instance;
        }


        //public static Dictionary<uint, Dictionary<uint, List<uint>>> ParseDicInDic(string csvPath)
        //{
        //    TextAsset ta = null;
        //        ta = Resources.Load<TextAsset>(csvPath);

        //    if (ta == null)
        //        return null;

        //    Dictionary<uint, Dictionary<uint, List<uint>>> result = new Dictionary<uint, Dictionary<uint, List<uint>>>();
        //    string[] lineArr = ta.text.Split(SYMBOL_LINE, StringSplitOptions.RemoveEmptyEntries);
        //    List<uint> titleDic = GameUtil.FormatListUint(lineArr[0], SYMBOL_FIELD);

        //    for (int i = 1, lineLen = lineArr.Length; i < lineLen; i++)//第一行是title所以i=1开始
        //    {
        //        Dictionary<uint, List<uint>> dicValue = new Dictionary<uint, List<uint>>();
        //        string[] strArr = lineArr[i].Trim().Split(SYMBOL_FIELD, StringSplitOptions.None);
        //        for (int j = 1; j < strArr.Length; j++)
        //        {
        //            dicValue.Add(titleDic[j], GameUtil.FormatListUint(strArr[j], SYMBOL_SECOND));
        //        }
        //        result.Add(uint.Parse(strArr[0]), dicValue);
        //    }
        //    return result;
        //}

    }
}