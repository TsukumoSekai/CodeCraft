using OfflineFantasy.GameCraft.Utility;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Design
{
    /// <summary>
    /// 基础Unity单例类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UnitySingleton<T> : MonoBehaviour where T : UnitySingleton<T>
    {
        private static T m_Instance;

        public static T Instance => m_Instance;

        protected virtual void Awake()
        {
            InitializeSingleton();
        }

        protected void InitializeSingleton()
        {
            if (m_Instance == null)
            {
                m_Instance = (T)this;

                DebugCraft.Log($"实例化Unity单例,  Transform:  {m_Instance.gameObject.name}_{m_Instance.transform.GetInstanceID()},  ClassType:  {m_Instance.GetType().ToString()}");
            }
            else
                DebugCraft.LogError($"存在多个Unity单例,  OldTransform:  {m_Instance.gameObject.name}_{m_Instance.transform.GetInstanceID()},  NewTransform:  {this.gameObject.name}_{this.gameObject.GetInstanceID()},  ClassType:  {m_Instance.GetType().ToString()}");
        }
    }
}