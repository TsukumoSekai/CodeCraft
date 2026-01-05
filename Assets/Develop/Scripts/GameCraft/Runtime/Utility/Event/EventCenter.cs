using System;
using System.Collections.Generic;

namespace OfflineFantasy.GameCraft.Utility.Event
{
    public static class EventCenter<EnumType>
    {
        /// <summary>
        /// 无参代理
        /// </summary>
        public delegate void CallBack();

        /// <summary>
        /// 一参代理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        public delegate void CallBack<T>(T arg);

        /// <summary>
        /// 二参代理
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public delegate void CallBack<T1, T2>(T1 arg1, T2 arg2);

        /// <summary>
        /// 三参代理
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        public delegate void CallBack<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);

        /// <summary>
        /// 四参数代理
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        public delegate void CallBack<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

        /// <summary>
        /// 事件字典
        /// </summary>
        private static Dictionary<string, Delegate> m_EventDict = new Dictionary<string, Delegate>();

        #region 私有静态方法

        /// <summary>
        /// 事件监听添加时
        /// </summary>
        /// <param name="_eventCode">事件编码</param>
        /// <param name="_callBack">委托事件</param>
        private static void OnListenerAdding(string _eventName, Delegate _callBack)
        {
            m_EventDict.TryAdd(_eventName, null);

            Delegate @delegate = m_EventDict[_eventName];

            if (@delegate != null && @delegate.GetType() != _callBack.GetType())
                DebugCraft.LogError($"添加监听错误:  尝试为事件 {_eventName} 添加不同类型的委托, 当前事件所对应的委托是 {@delegate.GetType()} ,要添加的委托是 {_callBack.GetType()} ");
        }

        /// <summary>
        /// 事件监听移除时
        /// </summary>
        /// <param name="_eventName"></param>
        /// <param name="_callBack"></param>
        private static void OnListenerRemoving(string _eventName, Delegate _callBack)
        {
            if (m_EventDict.ContainsKey(_eventName))
            {
                Delegate @delegate = m_EventDict[_eventName];

                if (@delegate == null)
                    DebugCraft.LogError($"移除监听错误:  事件 {_eventName} 没有对应的委托");
                else if (@delegate.GetType() != _callBack.GetType())
                    DebugCraft.LogError($"移除监听错误:  尝试为事件 {_eventName} 移除不同类型的委托, 当前事件所对应的委托为 {@delegate.GetType()} , 要移除的委托是 {_callBack.GetType()}");
            }
            else
            {
                DebugCraft.LogError($"移除监听错误:  没有事件码 {_eventName}");
            }
        }

        /// <summary>
        /// 事件监听移除后
        /// </summary>
        /// <param name="_eventCode"></param>
        private static void OnListenerRemoved(EnumType _eventCode)
        {
            string eventName = GetEnumName(_eventCode);

            if (m_EventDict[eventName] == null)
                m_EventDict.Remove(eventName);
        }

        /// <summary>
        /// 获取事件编码字符串
        /// </summary>
        /// <param name="_eventCode"></param>
        /// <returns></returns>
        private static string GetEnumName(EnumType _eventCode)
        {
            return $"{typeof(EnumType).ToString()}.{_eventCode.ToString()}";
            //return $"{_eventCode.GetType().ToString()}.{_eventCode.ToString()}";
        }

        #endregion

        #region 公共静态方法

        /// <summary>
        /// 无参监听添加
        /// </summary>
        /// <param name="_eventCode"></param>
        /// <param name="_callBack"></param>
        public static void AddListener(EnumType _eventCode, CallBack _callBack)
        {
            string eventName = GetEnumName(_eventCode);
            OnListenerAdding(eventName, _callBack);
            m_EventDict[eventName] = (CallBack)m_EventDict[eventName] + _callBack;
        }

        /// <summary>
        /// 一参监听添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_eventCode"></param>
        /// <param name="_callBack"></param>
        public static void AddListener<T>(EnumType _eventCode, CallBack<T> _callBack)
        {
            string eventName = GetEnumName(_eventCode);
            OnListenerAdding(eventName, _callBack);
            m_EventDict[eventName] = (CallBack<T>)m_EventDict[eventName] + _callBack;
        }

        /// <summary>
        /// 二参监听添加
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="_eventCode"></param>
        /// <param name="_callBack"></param>
        public static void AddListener<T0, T1>(EnumType _eventCode, CallBack<T0, T1> _callBack)
        {
            string eventName = GetEnumName(_eventCode);
            OnListenerAdding(eventName, _callBack);
            m_EventDict[eventName] = (CallBack<T0, T1>)m_EventDict[eventName] + _callBack;
        }

        /// <summary>
        /// 三参监听添加
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="_eventCode"></param>
        /// <param name="_callBack"></param>
        public static void AddListener<T0, T1, T2>(EnumType _eventCode, CallBack<T0, T1, T2> _callBack)
        {
            string eventName = GetEnumName(_eventCode);
            OnListenerAdding(eventName, _callBack);
            m_EventDict[eventName] = (CallBack<T0, T1, T2>)m_EventDict[eventName] + _callBack;
        }

        /// <summary>
        /// 四参监听添加
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="_eventCode"></param>
        /// <param name="_callBack"></param>
        public static void AddListener<T0, T1, T2, T3>(EnumType _eventCode, CallBack<T0, T1, T2, T3> _callBack)
        {
            string eventName = GetEnumName(_eventCode);
            OnListenerAdding(eventName, _callBack);
            m_EventDict[eventName] = (CallBack<T0, T1, T2, T3>)m_EventDict[eventName] + _callBack;
        }

        /// <summary>
        /// 无参监听移除
        /// </summary>
        /// <param name="_eventCode"></param>
        /// <param name="_callBack"></param>
        public static void RemoveListener(EnumType _eventCode, CallBack _callBack)
        {
            string eventName = GetEnumName(_eventCode);
            OnListenerRemoving(eventName, _callBack);
            m_EventDict[eventName] = (CallBack)m_EventDict[eventName] - _callBack;
            OnListenerRemoved(_eventCode);
        }

        /// <summary>
        /// 一参监听移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_eventCode"></param>
        /// <param name="_callBack"></param>
        public static void RemoveListener<T>(EnumType _eventCode, CallBack<T> _callBack)
        {
            string eventName = GetEnumName(_eventCode);
            OnListenerRemoving(eventName, _callBack);
            m_EventDict[eventName] = (CallBack<T>)m_EventDict[eventName] - _callBack;
            OnListenerRemoved(_eventCode);
        }

        /// <summary>
        /// 二参监听移除
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="_eventCode"></param>
        /// <param name="_callBack"></param>
        public static void RemoveListener<T0, T1>(EnumType _eventCode, CallBack<T0, T1> _callBack)
        {
            string eventName = GetEnumName(_eventCode);
            OnListenerRemoving(eventName, _callBack);
            m_EventDict[eventName] = (CallBack<T0, T1>)m_EventDict[eventName] - _callBack;
            OnListenerRemoved(_eventCode);
        }

        /// <summary>
        /// 三参监听移除
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="_eventCode"></param>
        /// <param name="_callBack"></param>
        public static void RemoveListener<T0, T1, T2>(EnumType _eventCode, CallBack<T0, T1, T2> _callBack)
        {
            string eventName = GetEnumName(_eventCode);
            OnListenerRemoving(eventName, _callBack);
            m_EventDict[eventName] = (CallBack<T0, T1, T2>)m_EventDict[eventName] - _callBack;
            OnListenerRemoved(_eventCode);
        }

        /// <summary>
        /// 四参监听移除
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="_eventCode"></param>
        /// <param name="_callBack"></param>
        public static void RemoveListener<T0, T1, T2, T3>(EnumType _eventCode, CallBack<T0, T1, T2, T3> _callBack)
        {
            string eventName = GetEnumName(_eventCode);
            OnListenerRemoving(eventName, _callBack);
            m_EventDict[eventName] = (CallBack<T0, T1, T2, T3>)m_EventDict[eventName] - _callBack;
            OnListenerRemoved(_eventCode);
        }

        /// <summary>
        /// 无参广播事件
        /// </summary>
        /// <param name="_eventCode"></param>
        public static void Broadcast(EnumType _eventCode)
        {
            string eventName = GetEnumName(_eventCode);
            if (m_EventDict.TryGetValue(eventName, out Delegate @delegate))
            {
                CallBack callBack = @delegate as CallBack;
                if (callBack != null)
                    callBack();
                else
                    DebugCraft.LogError($"广播事件错误,  事件 {eventName} 对应委托有不同的类型");
            }
        }

        /// <summary>
        /// 一参广播事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_eventCode"></param>
        /// <param name="_arg"></param>
        public static void Broadcast<T>(EnumType _eventCode, T _arg)
        {
            string eventName = GetEnumName(_eventCode);
            if (m_EventDict.TryGetValue(eventName, out Delegate @delegate))
            {
                CallBack<T> callBack = @delegate as CallBack<T>;
                if (callBack != null)
                    callBack(_arg);
                else
                    DebugCraft.LogError($"广播事件错误,  事件 {eventName} 对应委托有不同的类型");
            }
        }

        /// <summary>
        /// 二参广播事件
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="_eventCode"></param>
        /// <param name="_arg_0"></param>
        /// <param name="_arg_1"></param>
        public static void Broadcast<T0, T1>(EnumType _eventCode, T0 _arg_0, T1 _arg_1)
        {
            string eventName = GetEnumName(_eventCode);
            if (m_EventDict.TryGetValue(eventName, out Delegate @delegate))
            {
                CallBack<T0, T1> callBack = @delegate as CallBack<T0, T1>;
                if (callBack != null)
                    callBack(_arg_0, _arg_1);
                else
                    DebugCraft.LogError($"广播事件错误,  事件 {eventName} 对应委托有不同的类型");
            }
        }

        /// <summary>
        /// 三参广播事件
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="_eventCode"></param>
        /// <param name="_arg_0"></param>
        /// <param name="_arg_1"></param>
        /// <param name="_arg_2"></param>
        public static void Broadcast<T0, T1, T2>(EnumType _eventCode, T0 _arg_0, T1 _arg_1, T2 _arg_2)
        {
            string eventName = GetEnumName(_eventCode);
            if (m_EventDict.TryGetValue(eventName, out Delegate @delegate))
            {
                CallBack<T0, T1, T2> callBack = @delegate as CallBack<T0, T1, T2>;
                if (callBack != null)
                    callBack(_arg_0, _arg_1, _arg_2);
                else
                    DebugCraft.LogError($"广播事件错误,  事件 {eventName} 对应委托有不同的类型");
            }
        }

        /// <summary>
        /// 四参广播事件
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="_eventCode"></param>
        /// <param name="_arg_0"></param>
        /// <param name="_arg_1"></param>
        /// <param name="_arg_2"></param>
        /// <param name="_arg_3"></param>
        public static void Broadcast<T0, T1, T2, T3>(EnumType _eventCode, T0 _arg_0, T1 _arg_1, T2 _arg_2, T3 _arg_3)
        {
            string eventName = GetEnumName(_eventCode);
            if (m_EventDict.TryGetValue(eventName, out Delegate @delegate))
            {
                CallBack<T0, T1, T2, T3> callBack = @delegate as CallBack<T0, T1, T2, T3>;
                if (callBack != null)
                    callBack(_arg_0, _arg_1, _arg_2, _arg_3);
                else
                    DebugCraft.LogError($"广播事件错误,  事件 {eventName} 对应委托有不同的类型");
            }
        }

        /// <summary>
        /// 清除所有事件
        /// </summary>
        public static void Clear()
        {
            foreach (string eventName in m_EventDict.Keys)
            {
                m_EventDict[eventName] = null;
            }
            m_EventDict.Clear();
        }

        #endregion
    }
}