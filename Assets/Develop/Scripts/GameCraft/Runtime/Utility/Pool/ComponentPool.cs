using OfflineFantasy.GameCraft.Utility.Event;
using UnityEngine;
using UnityEngine.Pool;

namespace OfflineFantasy.GameCraft.Utility.Pool
{
    public class ComponentPool<T> where T : Behaviour
    {
        protected readonly ObjectPool<T> m_Pool;

        /// <summary>
        /// 统一释放组件,  由管理器调用
        /// </summary>
        protected readonly bool m_UniformRelease = true;

        /// <summary>
        /// 统一销毁组件,  由管理器调用
        /// </summary>
        protected readonly bool m_UniformDestroy = true;

        /// <summary>
        /// 组件所属物体
        /// </summary>
        public readonly GameObject m_Container;

        /// <summary>
        /// 当前激活中的组件数量
        /// </summary>
        public int ActivedObjectCount => m_Pool.CountActive;

        /// <summary>
        /// 当前未激活的组件数量
        /// </summary>
        public int InactivedObjectCount => m_Pool.CountInactive;

        /// <summary>
        /// 组件总数量
        /// </summary>
        public int TotalCount => m_Pool.CountAll;

        #region 保护方法

        protected virtual T OnCreatePoolObject()
        {
            return m_Container.AddComponent<T>();
        }

        protected virtual void OnGetPoolObject(T _component)
        {
            _component.enabled = true;
        }

        protected virtual void OnReleasePoolObject(T _component)
        {
            _component.enabled = false;
        }

        protected virtual void OnDestroyPoolObject(T _component)
        {
            Object.Destroy(_component);
        }

        #endregion

        #region 公共方法

        public ComponentPool(GameObject _container, bool _collectionCheck = true, int _initialSize = 0, int _maxSize = 100,
                             bool _autoRelease = true, bool _autoDestroy = true)
        {
            m_Container = _container;

            m_Pool = new ObjectPool<T>(OnCreatePoolObject,
                                       OnGetPoolObject,
                                       OnReleasePoolObject,
                                       OnDestroyPoolObject,
                                       _collectionCheck,
                                       _initialSize,
                                       _maxSize);

            m_UniformRelease = _autoRelease;
            m_UniformDestroy = _autoDestroy;

            if (m_UniformRelease)
                EventCenter<GlobalEventCode>.AddListener(GlobalEventCode.UniformReleasePooledObject, ReleaseAll);

            if (m_UniformDestroy)
                EventCenter<GlobalEventCode>.AddListener(GlobalEventCode.UniformDestroyPooledObject, Clear);
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <returns></returns>
        public virtual T Get()
        {
            return m_Pool.Get();
        }

        /// <summary>
        /// 释放指定组件
        /// </summary>
        /// <param name="_component"></param>
        public virtual void Release(T _component)
        {
            m_Pool.Release(_component);
        }

        /// <summary>
        /// 释放所有组件
        /// </summary>
        public void ReleaseAll()
        {
            foreach (T component in m_Container.GetComponents<T>())
            {
                Release(component);
            }
        }

        /// <summary>
        /// 销毁所有组件
        /// </summary>
        public virtual void Clear()
        {
            m_Pool.Clear();
        }

        #endregion
    }
}