using System;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class ComponentUtility
    {
        #region 拓展方法

        public static T GetOrAddComponent<T>(this Component _this) where T : Component
        {
            return _this.gameObject.GetOrAddComponent<T>();
        }

        public static bool TryGetComponentInChildren<T>(this Component _this, out T _component) where T : Component
        {
            _component = _this.gameObject.GetComponentInChildren<T>();
            return _component != null;
        }

        public static bool TryGetComponentInChildren(this Component _this, Type _type, out Component _component)
        {
            _component = _this.gameObject.GetComponentInChildren(_type);
            return _component != null;
        }

        public static bool TryGetComponentInParent<T>(this Component _this, out T _compoent) where T : Component
        {
            _compoent = _this.gameObject.GetComponentInParent<T>();
            return _compoent != null;
        }

        public static bool TryGetComponentInParent(this Component _this, Type _type, out Component _component)
        {
            _component = _this.gameObject.GetComponentInParent(_type);
            return _component != null;
        }

        public static T GetComponentInParents<T>(this Component _this, int _loopLimit = -1) where T : Component
        {
            return _this.gameObject.GetComponentInParents<T>(_loopLimit);
        }

        public static bool TryGetComponentInParents<T>(this Component _this, out T _component, int _loopLimit = -1) where T : Component
        {
            _component = _this.GetComponentInParents<T>(_loopLimit);
            return _component != null;
        }

        #endregion
    }
}