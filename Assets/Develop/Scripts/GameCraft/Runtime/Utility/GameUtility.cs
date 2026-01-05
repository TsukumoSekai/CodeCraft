using System;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class GameUtility
    {
        /// <summary>
        /// 设置游戏速率
        /// </summary>
        /// <param name="scale"></param>
        public static void SetGameTimeScale(float scale)
        {
            Time.timeScale = scale;
        }

        /// <summary>
        /// 获取GUID
        /// </summary>
        /// <returns></returns>
        public static uint FormatFunctionTypeGUID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToUInt32(buffer, 0);
        }
    }
}