using System;
using OfflineFantasy.GameCraft.Design;

namespace OfflineFantasy.GameCraft.Utility.Manager
{
    public class UpdateManager : UnitySingleton<UpdateManager>
    {
        public Action m_UpdateAction;
        public Action m_FixedUpdateAction;
        public Action m_LateUpdateAction;

        private void Update()
        {
            m_UpdateAction?.Invoke();
        }

        private void FixedUpdate()
        {
            m_FixedUpdateAction?.Invoke();
        }

        private void LateUpdate()
        {
            m_LateUpdateAction?.Invoke();
        }
    }
}