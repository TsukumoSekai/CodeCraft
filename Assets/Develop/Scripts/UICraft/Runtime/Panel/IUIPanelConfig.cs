using UnityEngine;

namespace OfflineFantasy.GameCraft.UI
{
    public interface IUIPanelConfig
    {
        public Camera UICamera { get; }

        public string GetUIPanelPath(string _uiPanelName);
    }
}