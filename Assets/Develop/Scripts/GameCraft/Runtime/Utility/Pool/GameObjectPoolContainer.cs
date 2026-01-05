using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility.Pool
{
    public class GameObjectPoolContainer : MonoBehaviour
    {
        public GameObjectPool GameObjectPool { private set; get; }

        [SerializeField]
        private GameObjectPoolConfig m_Config;

        private void Start()
        {
            GameObjectPool = PoolManager.Instance.GenerateGameObjectPool(m_Config);
        }
    }
}