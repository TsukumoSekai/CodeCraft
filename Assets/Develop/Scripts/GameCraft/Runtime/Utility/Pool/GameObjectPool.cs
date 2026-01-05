using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using OfflineFantasy.GameCraft.Utility.Event;
using UnityEngine;
using UnityEngine.Pool;

namespace OfflineFantasy.GameCraft.Utility.Pool
{
    public class GameObjectPool
    {
        private readonly ObjectPool<GameObject> m_Pool;

        /// <summary>
        /// 重置父物体,  在对象被释放时统一放在指定节点下
        /// </summary>
        private readonly bool m_ResetParent = true;

        /// <summary>
        /// 统一释放对象,  由管理器调用
        /// </summary>
        private readonly bool m_UniformRelease = true;

        /// <summary>
        /// 统一销毁对象,  由管理器调用
        /// </summary>
        private readonly bool m_UniformDestroy = true;

        //private List<GameObject> m_ActivatedObjectList = new List<GameObject>();
        private HashSet<GameObject> m_ActivatedObjectList = new();

        /// <summary>
        /// 预制体
        /// </summary>
        public readonly GameObject m_Prefab;

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

        /// <summary>
        /// 当前激活中的物体列表,  用于全部释放
        /// </summary>
        //public ReadOnlyCollection<GameObject> ActivatedObjectList => m_ActivatedObjectList.AsReadOnly();
        public IReadOnlyCollection<GameObject> ActivatedObjectList => m_ActivatedObjectList;

        public System.Action<GameObject> m_OnCreatePoolObjectAction;
        public System.Action<GameObject> m_OnGetPoolObjectAction;
        public System.Action<GameObject> m_OnReleasePoolObjectAction;
        public System.Action<GameObject> m_OnDestroyPoolObjectAction;

        ~GameObjectPool()
        {
            if (m_UniformRelease)
                EventCenter<GlobalEventCode>.RemoveListener(GlobalEventCode.UniformReleasePooledObject, ReleaseAll);

            if (m_UniformDestroy)
                EventCenter<GlobalEventCode>.RemoveListener(GlobalEventCode.UniformDestroyPooledObject, Clear);
        }

        #region 保护方法

        protected virtual GameObject OnCreatePoolObject()
        {
            //return Object.Instantiate(m_Prefab, m_Root, false);
            var gameObject = Object.Instantiate(m_Prefab, m_Root, false);

            m_OnCreatePoolObjectAction?.Invoke(gameObject);

            return gameObject;
        }

        protected virtual void OnGetPoolObject(GameObject _gameObject)
        {
            _gameObject.SetActive(true);
            m_ActivatedObjectList.Add(_gameObject);

            m_OnGetPoolObjectAction?.Invoke(_gameObject);
        }

        protected virtual void OnReleasePoolObject(GameObject _gameObject)
        {
            m_OnReleasePoolObjectAction?.Invoke(_gameObject);

            _gameObject.SetActive(false);

            m_ActivatedObjectList.Remove(_gameObject);

            if (m_ResetParent && m_Root != null)
                _gameObject.transform.SetParent(m_Root, false);
        }

        protected virtual void OnDestroyPoolObject(GameObject _gameObject)
        {
            m_OnDestroyPoolObjectAction?.Invoke(_gameObject);

            m_ActivatedObjectList.Remove(_gameObject);

            Object.Destroy(_gameObject);
        }

        #endregion

        #region 公共方法

        public GameObjectPool(GameObject _prefab, Transform _root, bool _collectionCheck = true, int _initialSize = 0, int _maxSize = 100,
                              bool _resetParent = true, bool _autoRelease = true, bool _autoDestroy = true)
        {
            m_Prefab = _prefab;
            m_Root = _root;

            m_Pool = new ObjectPool<GameObject>(OnCreatePoolObject,
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

        public GameObjectPool(GameObjectPoolConfig _config, Transform _root)
        {
            m_Prefab = _config.m_Prefab;
            m_Root = _root;

            m_Pool = new ObjectPool<GameObject>(OnCreatePoolObject,
                                                OnGetPoolObject,
                                                OnReleasePoolObject,
                                                OnDestroyPoolObject,
                                                _config.m_CollectionCheck,
                                                _config.m_InitialSize,
                                                _config.m_MaxSize);

            m_ResetParent = _config.m_ResetParent;
            m_UniformRelease = _config.m_AutoRelease;
            m_UniformDestroy = _config.m_AutoDestroy;

            if (m_UniformRelease)
                EventCenter<GlobalEventCode>.AddListener(GlobalEventCode.UniformReleasePooledObject, ReleaseAll);

            if (m_UniformDestroy)
                EventCenter<GlobalEventCode>.AddListener(GlobalEventCode.UniformDestroyPooledObject, Clear);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        public GameObject Get()
        {
            return m_Pool.Get();
        }

        /// <summary>
        /// 释放指定对象
        /// </summary>
        /// <param name="_gameObject"></param>
        public void Release(GameObject _gameObject)
        {
            if (_gameObject == null)
                return;

            m_Pool.Release(_gameObject);
        }

        /// <summary>
        /// 延迟释放指定对象
        /// </summary>
        /// <param name="_gameObject"></param>
        /// <param name="_millisecondsDelay"></param>
        /// <returns></returns>
        public async UniTask DelayRelease(GameObject _gameObject, int _millisecondsDelay)
        {
            await UniTask.Delay(_millisecondsDelay);

            Release(_gameObject);
        }

        /// <summary>
        /// 释放所有对象
        /// </summary>
        public void ReleaseAll()
        {
            //for (int i = m_ActivatedObjectList.Count - 1; i >= 0; i--)
            //{
            //    Release(m_ActivatedObjectList[i]);
            //}

            foreach (var go in m_ActivatedObjectList)
            {
                Release(go);
            }
        }

        /// <summary>
        /// 销毁所有对象
        /// </summary>
        public void Clear()
        {
            m_Pool.Clear();
        }

        #endregion
    }
}