//using OfflineFantasy.GameCraft.Model;
//using NetNorth.Utility;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Reflection;
//using UnityEngine;

//namespace OfflineFantasy.GameCraft.Utility
//{
//    public static class DatabaseUtility
//    {
//        public const string DatabasePath = "Database";

//        /// <summary>
//        /// 从数据库加载数据
//        /// </summary>
//        /// <typeparam name="K"></typeparam>
//        /// <typeparam name="V"></typeparam>
//        /// <param name="_path"></param>
//        /// <param name="_arrayName"></param>
//        /// <param name="_keyName"></param>
//        /// <returns></returns>
//        public static Dictionary<K, V> LoadData<K, V>(string _path, string _arrayName = "DataArray", string _keyName = "ID") where V : BaseData
//        {
//            Dictionary<K, V> dict = new Dictionary<K, V>();
//            //ScriptableObject scriptableObject = Language.Instance.Load<ScriptableObject>(ExcelConfig.ASSET_LOAD_PATH + _assetDataPath);
//            //TODO:  (TS)  封装Language

//            ScriptableObject scriptableObject = Resources.Load<ScriptableObject>(_path);

//            if (scriptableObject == null)
//            {
//                DebugCraft.LogError($"数据表不存在:  {_path},  {typeof(V).ToString()}");
//                return null;
//            }

//            IList list;

//            foreach (var soField in scriptableObject.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
//            {
//                if (soField.Name == _arrayName)
//                {
//                    list = soField.GetValue(scriptableObject) as IList;
//                    foreach (var value in list)
//                    {
//                        foreach (var valueField in value.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
//                        {
//                            if (valueField.Name == _keyName)
//                            {
//                                if (ExcelUtility.ParseVariable(typeof(K), valueField.GetValue(value).ToString(), out object key))
//                                {
//                                    if (!dict.TryAdd((K)key, (V)value))
//                                        DebugCraft.LogError($"表格数据ID重复:  {_path},  {valueField.GetValue(value).ToString()}");
//                                }
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    DebugCraft.LogWarning($"未找到对应名称的数组,  {_path},  {_arrayName}");
//                }
//            }

//            return dict;
//        }

//        /// <summary>
//        /// 解析数据成员
//        /// </summary>
//        /// <param name="_type"></param>
//        /// <param name="_valueString"></param>
//        /// <param name="_separator"></param>
//        /// <returns></returns>
//        public static object ParseDataMember(Type _type, string _valueString, char[] _separator)
//        {
//            object dataMember = Activator.CreateInstance(_type);

//            List<MemberInfo> memberInfoList = new List<MemberInfo>();
//            memberInfoList.AddRange(_type.GetFields(BindingFlags.Public | BindingFlags.Instance));
//            memberInfoList.AddRange(_type.GetProperties(BindingFlags.Public | BindingFlags.Instance));

//            string[] elementArray = _valueString.Split(_separator);
//            DataMemberIndexAttribute dataMemberIndex;

//            foreach (MemberInfo memberInfo in memberInfoList)
//            {
//                dataMemberIndex = memberInfo.GetCustomAttribute<DataMemberIndexAttribute>(false);

//                if (dataMemberIndex != null)
//                {
//                    if (memberInfo is FieldInfo)
//                    {
//                        if (ExcelUtility.ParseVariable((memberInfo as FieldInfo).FieldType, elementArray[dataMemberIndex.Index], out object valueObject))
//                            (memberInfo as FieldInfo).SetValue(dataMember, valueObject);
//                        else
//                            DebugCraft.LogError($"数据解析失败,  键:  {memberInfo.Name},  值:  {elementArray[dataMemberIndex.Index]}");
//                    }
//                    else if (memberInfo is PropertyInfo)
//                    {
//                        if (ExcelUtility.ParseVariable((memberInfo as PropertyInfo).PropertyType, elementArray[dataMemberIndex.Index], out object valueObject))
//                            (memberInfo as PropertyInfo).SetValue(dataMember, valueObject);
//                        else
//                            DebugCraft.LogError($"数据解析失败,  键:  {memberInfo.Name},  值:  {elementArray[dataMemberIndex.Index]}");
//                    }
//                }
//            }

//            return dataMember;
//        }

//#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
//        [UnityEditor.MenuItem("FrameCraft/Tools/ClearDataCache")]
//#endif
//        public static void ClearDataCache()
//        {
//            Type[] dataTypeArr = typeof(BaseData).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(BaseData)) && !t.IsAbstract).ToArray();

//            foreach (Type dataType in dataTypeArr)
//            {
//                FieldInfo fieldInfo = dataType.GetField("m_DataDict", BindingFlags.NonPublic | BindingFlags.Static);

//                if (fieldInfo == null)
//                    continue;

//                fieldInfo.SetValue(null, null);

//                //DebugCraft.Log($"清理数据缓存:  {dataType.Name}");
//            }

//            DebugCraft.Log($"数据缓存清理完毕");
//        }
//    }

//    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
//    public class DataMemberIndexAttribute : Attribute
//    {
//        public int Index;

//        public DataMemberIndexAttribute(int _index)
//        {
//            Index = _index;
//        }
//    }
//}