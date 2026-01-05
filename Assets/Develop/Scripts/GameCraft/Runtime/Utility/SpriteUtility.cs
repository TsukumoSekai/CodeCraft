using System.Collections.Generic;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class SpriteUtility
    {
        private static Dictionary<string, Sprite> m_SpriteDict = new Dictionary<string, Sprite>();

        public static Sprite LoadSprite(string _path)
        {
            if (!m_SpriteDict.TryGetValue(_path, out Sprite sprite))
            {
                sprite = Resources.Load<Sprite>(_path);

                if (sprite == null)
                {
                    Debug.LogError($"精灵不存在:  {_path}");
                    return null;
                }

                m_SpriteDict.Add(_path, sprite);
            }

            return sprite;
        }
    }
}