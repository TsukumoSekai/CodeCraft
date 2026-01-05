using System;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class AssetDatabaseUtility
    {
        private const char m_AbsolutePathSeparator = '\\';
        private const char m_AssetPathSeparator = '/';
        private const string m_AssetPathMark = "Assets";

        //AbsolutePath:  X:\XXXX\Assets/XXXX
        //AssetPath:  Assets/XXXX
        public static string AbsolutePathToAssetPath(string _absolutePath)
        {

            return _absolutePath.Substring(_absolutePath.IndexOf(m_AssetPathMark)).Replace(m_AbsolutePathSeparator, m_AssetPathSeparator);
        }

        public static string AssetPathToAbsolutePath(string _assetPath)
        {
            return ($"{Application.dataPath}{_assetPath.Replace(m_AssetPathMark, string.Empty)}").Replace(m_AssetPathSeparator, m_AbsolutePathSeparator);
        }

        /// <summary>
        /// 从字符串中截取资产路径
        /// </summary>
        /// <param name="_sourcePath"></param>
        /// <returns></returns>
        public static string CutOutAssetsPath(string _sourcePath, string _separator, bool _reserveFileName = true, bool _reserveSuffix = true)
        {
            if (string.IsNullOrEmpty(_sourcePath))
            {
                DebugCraft.LogError("路径为空");
                return string.Empty;
            }

            string[] fullPathArray = _sourcePath.Split(_separator);
            int assetsPathIndex = Array.IndexOf(fullPathArray, "Assets");

            if (assetsPathIndex == -1)
            {
                Debug.LogError("路径不在项目文件夹中");
                return string.Empty;
            }

            string assetsPath = string.Empty;
            string clip;

            for (int i = assetsPathIndex; i < fullPathArray.Length; i++)
            {
                clip = fullPathArray[i];

                if (i == fullPathArray.Length - 1 && clip.Contains('.'))
                {
                    if (_reserveFileName)
                        assetsPath += _reserveSuffix ?
                                      $"/{clip}" :
                                      $"/{clip.Split('.')[0]}";

                    break;
                }

                if (i != assetsPathIndex)
                    assetsPath += "/";

                assetsPath += clip;
            }

            return assetsPath;
        }

        /// <summary>
        /// 从字符串中截取资产路径
        /// </summary>
        /// <param name="_sourcePath"></param>
        /// <param name="_assetsPath"></param>
        /// <returns></returns>
        public static bool TryCutOutAssetsPath(string _sourcePath, string _separator, out string _assetsPath, bool _reserveFileName = true, bool _reserveSuffix = true)
        {
            _assetsPath = CutOutAssetsPath(_sourcePath, _separator);
            return _assetsPath != null;
        }

        /// <summary>
        /// 从字符串中截取资源路径
        /// </summary>
        /// <param name="_sourcePath"></param>
        /// <param name="_separator"></param>
        /// <returns></returns>
        public static string CutOutResourcesPath(string _sourcePath, string _separator)
        {
            if (string.IsNullOrEmpty(_sourcePath))
            {
                DebugCraft.LogError("路径为空");
                return string.Empty;
            }

            string[] fullPathArray = _sourcePath.Split(_separator);
            int resourcesPathIndex = Array.IndexOf(fullPathArray, "Resources");

            if (resourcesPathIndex == -1)
            {
                Debug.LogError($"路径不在资源文件夹中:  {_sourcePath}");
                return string.Empty;
            }

            string resourcesPath = string.Empty;
            string clip;

            for (int i = resourcesPathIndex + 1; i < fullPathArray.Length; i++)
            {
                clip = fullPathArray[i];

                if (i == fullPathArray.Length - 1 && clip.Contains('.'))
                    break;

                if (i != resourcesPathIndex + 1)
                    resourcesPath += "/";

                resourcesPath += fullPathArray[i];
            }

            return resourcesPath;
        }

        /// <summary>
        /// 从字符串中截取资源路径
        /// </summary>
        /// <param name="_sourcePath"></param>
        /// <param name="_separator"></param>
        /// <param name="_resourcePath"></param>
        /// <returns></returns>
        public static bool TryCutOutResourcesPath(string _sourcePath, string _separator, out string _resourcePath)
        {
            _resourcePath = CutOutResourcesPath(_sourcePath, _separator);
            return _resourcePath != null;
        }

        public static string GetPathWithoutExtension(string _sourcePath)
        {
            int index = _sourcePath.LastIndexOf('.');

            return _sourcePath.Remove(index);
        }
    }
}