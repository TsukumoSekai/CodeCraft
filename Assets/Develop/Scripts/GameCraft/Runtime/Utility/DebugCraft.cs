using System;
using System.Collections.Generic;
using UnityEngine;
using OfflineFantasy.GameCraft.Design;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
using UnityEditor;
#endif

namespace OfflineFantasy.GameCraft.Utility
{
    //[Obsolete("此类已过期,  用DebugCraft<LogModuleType>代替")]
    public class DebugCraft : SystemSingleton<DebugCraft>
    {
        public static void Log(object _content, LogType _logType = LogType.Null)
        {
            Debug.Log(_content);
        }

        public static void LogWarning(object _content, LogType _logType = LogType.Null)
        {
            Debug.LogWarning(_content);
        }

        public static void LogError(object _content, LogType _logType = LogType.Null)
        {
            Debug.LogError(_content);
        }
    }

    /// <summary>
    /// TODO:导出日志
    /// </summary>
    /// <typeparam name="LogModuleType"></typeparam>
    public static class DebugCraft<LogModuleType> where LogModuleType : Enum
    {
        private static Dictionary<LogLevelType, bool> m_EnableLogLevelDict = new Dictionary<LogLevelType, bool>()
        {
            { LogLevelType.Info, true },
            { LogLevelType.Warning, true },
            { LogLevelType.Error, true },
        };

        private static Dictionary<LogModuleType, bool> m_EnableLogModuleDict;

        private static List<LogPackage<LogModuleType>> m_LogPackageList = new List<LogPackage<LogModuleType>>();

        private static List<string> m_InitiatorList = new List<string>();

        private static List<string> m_TargetList = new List<string>();

        /// <summary>
        /// 是否打印日志
        /// </summary>
        public static bool m_EnableLog = true;

        /// <summary>
        /// 允许打印的日志等级类型
        /// </summary>
        public static Dictionary<LogLevelType, bool> EnableLogLevelDict => m_EnableLogLevelDict;

        /// <summary>
        /// 允许打印的日志模块字典
        /// </summary>
        public static Dictionary<LogModuleType, bool> EnableLogModuleDict
        {
            get
            {
                if (m_EnableLogModuleDict == null)
                {
                    m_EnableLogModuleDict = new Dictionary<LogModuleType, bool>();

                    foreach (LogModuleType item in Enum.GetValues(typeof(LogModuleType)))
                    {
                        m_EnableLogModuleDict.Add(item, true);
                    }
                }

                return m_EnableLogModuleDict;
            }
        }

        /// <summary>
        /// 日志包列表
        /// </summary>
        public static List<LogPackage<LogModuleType>> LogPackageList => m_LogPackageList;

        private static void Log(object _content, LogLevelType _logLevelType, LogModuleType _logModuleType = default, object _initiator = null, object _target = null)
        {
            LogPackage<LogModuleType> logPackage = new LogPackage<LogModuleType>(_content, _logLevelType, _logModuleType, _initiator, _target);

            m_LogPackageList.Add(logPackage);

            if (_initiator != null)
                m_InitiatorList.TryAdd(_initiator.ToString());

            if (_target != null)
                m_TargetList.TryAdd(_target.ToString());

            if (m_EnableLog && EnableLogLevelDict[_logLevelType] && EnableLogModuleDict[_logModuleType])
            {
                switch (_logLevelType)
                {
                    case LogLevelType.Info:
                        Debug.Log($"[{_logModuleType}]{_content}");
                        break;
                    case LogLevelType.Warning:
                        Debug.LogWarning($"[{_logModuleType}]{_content}");
                        break;
                    case LogLevelType.Error:
                        Debug.LogError($"[{_logModuleType}]{_content}");
                        break;
                }
            }
        }

        public static void Log(object _content, LogModuleType _logModuleType, object _initiator = null, object _target = null)
        {
            Log(_content, LogLevelType.Info, _logModuleType, _initiator, _target);
        }

        public static void LogWarning(object _content, LogModuleType _logModuleType, object _initiator = null, object _target = null)
        {
            Log(_content, LogLevelType.Warning, _logModuleType, _initiator, _target);
        }

        public static void LogError(object _content, LogModuleType _logModuleType, object _initiator = null, object _target = null)
        {
            Log(_content, LogLevelType.Error, _logModuleType, _initiator, _target);

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN

            //System.Type consoleWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");

            //EditorWindow.FocusWindowIfItsOpen(consoleWindowType);

            //EditorWindow.GetWindow(consoleWindowType).ShowNotification(new GUIContent($"报错了"));
#endif
        }

        public static void ClearLog()
        {
            m_LogPackageList.Clear();
        }
    }

    [Serializable]
    public readonly struct LogPackage<LogModuleType> where LogModuleType : Enum
    {
        public readonly DateTime m_LogTime;

        public readonly object m_Content;

        public readonly LogLevelType m_LogLevelType;

        public readonly LogModuleType m_LogModuleType;

        public readonly object m_Initiator;

        public readonly object m_Target;

        public LogPackage(object _content, LogLevelType _logLevelType, LogModuleType _logModuleType = default, object _initiator = null, object _target = null)
        {
            m_LogTime = DateTime.Now;
            m_Content = _content;
            m_LogLevelType = _logLevelType;
            m_LogModuleType = _logModuleType;
            m_Initiator = _initiator;
            m_Target = _target;
        }

        public override string ToString()
        {
            string log = $"[{m_LogTime.ToString("yyyy/MM/dd HH:mm:ss")}][{m_LogLevelType}][{m_LogModuleType}]";

            if (m_Initiator != null)
                log += $"[Initiator:  {m_Initiator}]";

            if (m_Target != null)
                log += $"[Target:  {m_Target}]";

            log += m_Content;

            return log;
        }

        public Texture2D LogLevelIcon
        {
            get
            {
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
                switch (m_LogLevelType)
                {
                    case LogLevelType.Info:
                        return EditorGUIUtility.FindTexture("d_console.infoicon.sml");
                    case LogLevelType.Warning:
                        return EditorGUIUtility.FindTexture("d_console.warnicon.sml");
                    case LogLevelType.Error:
                        return EditorGUIUtility.FindTexture("d_console.erroricon.sml");
                }
#endif
                return null;
            }
        }

        public string InitiatorName => m_Initiator != null ? m_Initiator.ToString() : "Null";

        public string TargetName => m_Target != null ? m_Target.ToString() : "Null";

        public string ContentString => m_Content != null ? m_Content.ToString() : string.Empty;
    }

    [System.Flags]
    public enum LogType
    {
        Null = 1,
        System = 2,
        Battle = 4,
        Item = 8,
        Walkingmap = 16,
    }

    [System.Flags]
    public enum LogLevelType
    {
        Info = 1,
        Warning = 2,
        Error = 4,
    }
}