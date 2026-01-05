using System.Collections.Generic;
using OfflineFantasy.GameCraft.Utility.Event;
using UnityEngine;
using UnityEngine.Pool;

namespace OfflineFantasy.GameCraft.Utility.Pool
{
    public class BasePool<T> where T : Component
    {
        protected readonly ObjectPool<T> m_Pool;

        /// <summary>
        /// 重置父物体,  在对象被释放时统一放在指定节点下
        /// </summary>
        protected readonly bool m_ResetParent = true;

        /// <summary>
        /// 统一释放对象,  由管理器调用
        /// </summary>
        protected readonly bool m_UniformRelease = true;

        /// <summary>
        /// 统一销毁对象,  由管理器调用
        /// </summary>
        protected readonly bool m_UniformDestroy = true;

        /// <summary>
        /// 当前激活中的物体列表,  用于全部释放
        /// </summary>
        protected List<T> m_ActivedObjectList = new List<T>();

        /// <summary>
        /// 预制体
        /// </summary>
        public readonly T m_Prefab;

        /// <summary>
        /// 根节点,  用于存放被释放的物体
        /// </summary>
        public readonly Transform m_Root;

        /// <summary>
        /// 当前激活中的对象数量
        /// </summary>
        public int ActivedObjectCount => m_Pool.CountActive;

        /// <summary>
        /// 当前未激活的对象数量
        /// </summary>
        public int InactivedObjectCount => m_Pool.CountInactive;

        /// <summary>
        /// 对象总数量
        /// </summary>
        public int TotalCount => m_Pool.CountAll;

        ~BasePool()
        {
            if (m_UniformRelease)
                EventCenter<GlobalEventCode>.RemoveListener(GlobalEventCode.UniformReleasePooledObject, ReleaseAll);

            if (m_UniformDestroy)
                EventCenter<GlobalEventCode>.RemoveListener(GlobalEventCode.UniformDestroyPooledObject, Clear);
        }

        #region 保护方法

        protected virtual T OnCreatePoolObject()
        {
            return Object.Instantiate(m_Prefab, m_Root, false).GetComponentInChildren<T>();
        }

        protected virtual void OnGetPoolObject(T _component)
        {
            _component.gameObject.SetActive(true);

            m_ActivedObjectList.Add(_component);
        }

        protected virtual void OnReleasePoolObject(T _component)
        {
            _component.gameObject.SetActive(false);

            m_ActivedObjectList.Remove(_component);
        }

        protected virtual void OnDestroyPoolObject(T _component)
        {
            m_ActivedObjectList.Remove(_component);

            Object.Destroy(_component.gameObject);
        }

        #endregion

        #region 公共方法

        public BasePool(T _prefab, Transform _root, bool _collectionCheck = true, int _initialSize = 0, int _maxSize = 100,
                        bool _resetParent = true, bool _autoRelease = true, bool _autoDestroy = true)
        {
            m_Prefab = _prefab;
            m_Root = _root;

            m_Pool = new ObjectPool<T>(OnCreatePoolObject,
                                       OnGetPoolObject,
                                       OnReleasePoolObject,
                                       OnDestroyPoolObject,
                                       _collectionCheck,
                                       _initialSize,
                                       _maxSize);

            m_ResetParent = _resetParent;
            m_UniformRelease = _autoRelease;
            m_UniformDestroy = _autoDestroy;

            if (m_UniformRelease)
                EventCenter<GlobalEventCode>.AddListener(GlobalEventCode.UniformReleasePooledObject, ReleaseAll);

            if (m_UniformDestroy)
                EventCenter<GlobalEventCode>.AddListener(GlobalEventCode.UniformDestroyPooledObject, Clear);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        public virtual T Get()
        {
            return m_Pool.Get();
        }

        /// <summary>
        /// 释放指定对象
        /// </summary>
        /// <param name="_object"></param>
        public virtual void Release(T _object)
        {
            m_Pool.Release(_object);
            _object = null;
        }

        /// <summary>
        /// 释放所有对象
        /// </summary>
        public void ReleaseAll()
        {
            foreach (T @object in m_ActivedObjectList)
            {
                Release(@object);
            }
        }

        /// <summary>
        /// 销毁所有对象
        /// </summary>
        public virtual void Clear()
        {
            m_Pool.Clear();
        }

        #endregion
    }
}