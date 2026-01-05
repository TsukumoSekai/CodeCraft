namespace OfflineFantasy.GameCraft.Utility.Event
{
    /// <summary>
    /// 全局事件枚举码
    /// </summary>
    public enum GlobalEventCode
    {
        /// <summary>
        /// 保存完成
        /// </summary>
        SaveComplete,
        /// <summary>
        /// 加载完成
        /// </summary>
        LoadComplete,
        /// <summary>
        /// 切换语言
        /// </summary>
        ChangeLanguage,
        /// <summary>
        /// 统一释放对象池对象
        /// </summary>
        UniformReleasePooledObject,
        /// <summary>
        /// 统一销毁对象池对象
        /// </summary>
        UniformDestroyPooledObject,
        /// <summary>
        /// 开始游戏
        /// </summary>
        StartGame,
        /// <summary>
        /// 停止游戏
        /// </summary>
        StopGame,
        /// <summary>
        /// 重置游戏
        /// </summary>
        ResetGame,
    }

    /// <summary>
    /// UI事件枚举码
    /// </summary>
    public enum UIEventCode
    {
        /// <summary>
        /// 输入文字
        /// </summary>
        InputText,
    }
}