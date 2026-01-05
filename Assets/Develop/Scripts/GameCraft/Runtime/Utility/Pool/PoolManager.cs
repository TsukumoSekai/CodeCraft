using OfflineFantasy.GameCraft.Design;
using OfflineFantasy.GameCraft.Utility.Event;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility.Pool
{
    /// <summary>
    /// TODO:对象池查重,
    /// TODO:绘制InitialPool,
    /// NOTE:既可以通过PoolManager直接设置在游戏开始时创建的对象池,  也可以在各个代码中随时创建对象池
    /// </summary>
    public class PoolManager : UnitySingleton<PoolManager>
    {
        [SerializeField]
        private Transform m_PublicRoot;

        [SerializeField]
        private GameObjectPoolConfig[] m_ConfigArray;

        //所有对象池字典，目前用不着
        //private Dictionary<string, GameObjectPool> m_GameObjectPoolDict = new Dictionary<string, GameObjectPool>();

        protected override void Awake()
        {
            base.Awake();

            if (m_PublicRoot == null)
            {
                GameObject go = new GameObject("PublicPoolRoot");
                go.transform.SetParent(this.transform, false);

                m_PublicRoot = go.transform;
            }

            foreach (GameObjectPoolConfig config in m_ConfigArray)
            {
                GenerateGameObjectPool(config);
            }
        }

        /// <summary>
        /// 生成对象池
        /// </summary>
        /// <param name="_config"></param>
        /// <returns></returns>
        public GameObjectPool GenerateGameObjectPool(GameObjectPoolConfig _config)
        {
            GameObjectPool pool;

            if (_config.m_Root == null)
            {
                GameObject container = new GameObject($"{_config.m_Prefab.name}Pool");
                container.transform.SetParent(m_PublicRoot, false);
                pool = new GameObjectPool(_config.m_Prefab, container.transform, true, _config.m_InitialSize, _config.m_MaxSize);
            }
            else
                pool = new GameObjectPool(_config.m_Prefab, _config.m_Root, true, _config.m_InitialSize, _config.m_MaxSize);

            //if (!m_GameObjectPoolDict.TryAdd(pool.m_Prefab.name, pool))
            //    DebugCraft<LogModuleType>.LogError($"存在同名对象池:  {pool.m_Prefab.name}", LogModuleType.Debug);

            if (_config.m_AutoSetPrefabInactive)
                _config.m_Prefab.gameObject.SetActive(false);

            return pool;
        }

        public GameObjectPool GenerateGameObjectPool(GameObject _prefab, Transform _root = null, bool _collectionCheck = true, int _initialSize = 0, int _maxSize = 100,
                                                     bool _resetParent = true, bool _autoRelease = true, bool _autoDestroy = true)
        {
            if (_root == null)
                _root = m_PublicRoot;

            GameObjectPool pool = new GameObjectPool(_prefab, _root, _collectionCheck, _initialSize, _maxSize, _resetParent, _autoRelease, _autoDestroy);

            return pool;
        }

        /// <summary>
        /// 释放所有对象池对象
        /// </summary>
        public void ReleaseAllPooledObject()
        {
            EventCenter<GlobalEventCode>.Broadcast(GlobalEventCode.UniformReleasePooledObject);
        }

        /// <summary>
        /// 销毁所有对象池对象
        /// </summary>
        public void DestroyAllPooledObject()
        {
            EventCenter<GlobalEventCode>.Broadcast(GlobalEventCode.UniformDestroyPooledObject);
        }
    }
}