using System;
using UnityEngine;

namespace OfflineFantasy.GameCraft.UI
{
    [Serializable]
    public class UIPanelInfo : ISerializationCallbackReceiver
    {
        public string Type;
        public string Path;

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize() { }
    }
}