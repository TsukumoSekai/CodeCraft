using UnityEngine;

namespace OfflineFantasy.GameCraft.Design
{
    /// <summary>
    /// 基础系统单例类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SystemSingleton<T> where T : SystemSingleton<T>, new()
    {
        private static T m_Instance;

        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new T();
                    m_Instance.Init();
                    Debug.Log($"实例化System单例, Class: { m_Instance.GetType().ToString() }");
                }

                return m_Instance;
            }
        }

        public virtual void Init() { }
    }
}