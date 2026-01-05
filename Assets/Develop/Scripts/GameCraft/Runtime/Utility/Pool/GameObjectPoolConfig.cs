using System;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility.Pool
{
    [Serializable]
    public class GameObjectPoolConfig
    {
        public GameObject m_Prefab;

        public Transform m_Root;

        public bool m_CollectionCheck = true;

        public int m_InitialSize = 0;

        public int m_MaxSize = 100;

        public bool m_ResetParent = false;

        public bool m_AutoRelease = false;

        public bool m_AutoDestroy = false;

        public bool m_AutoSetPrefabInactive = true;
    }
}