using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class TransformExtension
    {
        #region 拓展方法

        public static int GetActiveChildCount(this Transform _self, bool _activeSelf = true)
        {
            int childCount = _self.childCount;
            int count = 0;

            for (var i = 0; i < childCount; i++)
            {
                if ((_activeSelf && _self.GetChild(i).gameObject.activeSelf) ||
                    (!_activeSelf && _self.GetChild(i).gameObject.activeInHierarchy))
                    count++;
            }

            return count;
        }

        #endregion
    }
}