using System;
using System.Collections.Generic;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class GameObjectUtility
    {
        /// <summary>
        /// 摧毁指定物体的所有子物体
        /// </summary>
        /// <param name="_parent"></param>
        /// <param name="_isImmediate"></param>
        /// <param name="_ignoreInactived"></param>
        public static void DestroyAllChildren(Transform _parent, bool _isImmediate = false, bool _ignoreInactived = false)
        {
            int childCount = _parent.transform.childCount;
            GameObject go;

            for (int i = childCount - 1; i >= 0; i--)
            {
                go = _parent.transform.GetChild(i).gameObject;

                if (_ignoreInactived && !go.activeSelf)
                    continue;

                if (_isImmediate)
                    GameObject.DestroyImmediate(go);
                else
                    GameObject.Destroy(go);
            }
        }

        /// <summary>
        /// 设置指定物体的所有子物体层级
        /// </summary>
        /// <param name="_gameObject"></param>
        /// <param name="_layerName"></param>
        public static void SetLayer(GameObject _gameObject, string _layerName)
        {
            int layer = LayerMask.NameToLayer(_layerName);

            foreach (Transform tf in _gameObject.GetComponentsInChildren<Transform>(true))
            {
                tf.gameObject.layer = layer;
            }
        }

        public static void SetActive(GameObject _gameObject, bool _active)
        {
            if (_gameObject.activeSelf != _active)
            {
                //Debug.Log($"物体激活状态变更:  {_gameObject.name}({_gameObject.GetInstanceID()}),  {_active}");
                _gameObject.SetActive(_active);
            }
        }

        public static void SetActive(GameObject[] _gameObjectArray, bool _active)
        {
            if (_gameObjectArray != null)
                foreach (GameObject gameObject in _gameObjectArray)
                {
                    SetActive(gameObject, _active);
                }
        }

        #region 查找物体

        /// <summary>
        /// 获取指定物体的所有子物体的字典
        /// </summary>
        /// <param name="_gameObject"></param>
        /// <returns></returns>
        public static Dictionary<string, GameObject> GetChildGameObjectDict(GameObject _gameObject, bool _inclusiveSelf = false, Predicate<GameObject> _condition = null)
        {
            Dictionary<string, GameObject> dict = new Dictionary<string, GameObject>();

            if (_inclusiveSelf)
                dict.Add(_gameObject.name, _gameObject);

            foreach (Transform tf in _gameObject.GetComponentsInChildren<Transform>(true))
            {
                if (_condition != null && !_condition(tf.gameObject))
                    continue;

                if (!dict.TryAdd(tf.name.Remove(0, 2), tf.gameObject))
                {
                    string objectPath = tf.name;

                    Transform parent = tf.parent;

                    while (parent != null)
                    {
                        objectPath = $"{parent.name}/{objectPath}";
                        parent = parent.parent;
                    }

                    DebugCraft.LogWarning($"有同名物体:  {objectPath}");
                }
            }

            return dict;
        }

        /// <summary>
        /// 将游戏对象字典转换为组件字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Dictionary<string, T> ConvertGameObjectToComponent<T>(Dictionary<string, GameObject> target) where T : Component
        {
            Dictionary<string, T> dict = new Dictionary<string, T>();
            T component;

            foreach (KeyValuePair<string, GameObject> kv in target)
            {
                if (kv.Value.TryGetComponent(out component))
                {
                    dict.Add(kv.Key, component);
                }
            }

            return dict;
        }

        /// <summary>
        /// 返回父物体中第一个激活中的子物体
        /// </summary>
        /// <param name="_parent"></param>
        /// <returns></returns>
        public static GameObject GetFirstActiveObject(Transform _parent)
        {
            for (int i = 0; i < _parent.childCount; i++)
            {
                if (_parent.GetChild(i).gameObject.activeSelf)
                    return _parent.GetChild(i).gameObject;
            }

            return null;
        }

        /// <summary>
        /// 返回父物体中最后一个激活中的子物体
        /// </summary>
        /// <param name="_parent"></param>
        /// <returns></returns>
        public static GameObject GetLastActiveObject(Transform _parent)
        {
            for (int i = _parent.childCount - 1; i >= 0; i--)
            {
                if (_parent.GetChild(i).gameObject.activeSelf)
                    return _parent.GetChild(i).gameObject;
            }

            return null;
        }

        #endregion

        #region 拓展方法

        public static bool IsPrefab(this GameObject _gameObject)
        {
            return !_gameObject.scene.IsValid() &&
                   !_gameObject.scene.isLoaded &&
                   _gameObject.GetInstanceID() >= 0;
        }

        #region 查找组件

        /// <summary>
        /// 获取或添加组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_go"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject _go) where T : Component
        {
            if (!_go.TryGetComponent(out T _component))
                _component = _go.AddComponent<T>();
            return _component;
        }

        /// <summary>
        /// 尝试从所有子物体中查找单个组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_go"></param>
        /// <param name="_component"></param>
        /// <returns></returns>
        public static bool TryGetComponentInChildren<T>(this GameObject _go, out T _component) where T : Component
        {
            _component = _go.GetComponentInChildren<T>();

            return _component != null;
        }

        /// <summary>
        /// 尝试从父物体中查找单个组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_go"></param>
        /// <param name="_component"></param>
        /// <returns></returns>
        public static bool TryGetComponentInParent<T>(this GameObject _go, out T _component) where T : Component
        {
            _component = _go.GetComponentInParent<T>();
            return _component != null;
        }

        /// <summary>
        /// 从所有父物体中查找单个组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_go"></param>
        /// <returns></returns>
        public static T GetComponentInParents<T>(this GameObject _go, int _loopLimit = -1) where T : Component
        {
            Transform transform = _go.transform;
            int loopCount = 0;

            while (transform.parent != null && (_loopLimit != -1 || loopCount >= _loopLimit))
            {
                transform = transform.parent;

                if (transform.TryGetComponent(out T component))
                    return component;

                loopCount++;
            }

            return default;
        }

        /// <summary>
        /// 尝试从所有父物体中查找单个组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_go"></param>
        /// <param name="_component"></param>
        /// <returns></returns>
        public static bool TryGetComponentInParents<T>(this GameObject _go, out T _component, int _loopLimit = -1) where T : Component
        {
            _component = GetComponentInParents<T>(_go, _loopLimit);
            return _component != null;
        }

        #endregion

        /// <summary>
        /// 获取对象在场景中的路径
        /// </summary>
        /// <param name="_go"></param>
        /// <returns></returns>
        public static string GetGameObjectPath(this GameObject _go)
        {
            string path = _go.name;
            Transform parent = _go.transform.parent;

            while (parent != null)
            {
                path = $"{parent.name}/{path}";
                parent = parent.parent;
            }

            return path;
        }

        #endregion
    }
}