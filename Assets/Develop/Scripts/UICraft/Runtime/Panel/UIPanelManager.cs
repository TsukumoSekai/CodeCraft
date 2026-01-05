#if ENABLE_UNITASK
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using OfflineFantasy.GameCraft.Design;
using OfflineFantasy.GameCraft.Utility.Event;
using UnityEngine;

namespace OfflineFantasy.GameCraft.UI
{
    public partial class UIPanelManager : UnitySingleton<UIPanelManager>
    {
        #region 常量

        /// <summary>
        /// 世界坐标系单位尺度映射到屏幕坐标系中的距离, 即世界坐标系中1米等于屏幕坐标系中54米, 公式为CanvasScaler.referenceResolution.y / 20
        /// </summary>
        public const float m_ReferenceDistancePerUnit = 54f;

        #endregion

        #region 私有字段

        private IUIPanelConfig m_Config;

        /// <summary>
        /// 已实例化的面板字典
        /// </summary>
        private Dictionary<string, BasePanel> m_UIPanelDict = new Dictionary<string, BasePanel>();

        /// <summary>
        /// 显示中的面板列表
        /// </summary>
        private LinkedList<BasePanel> m_PanelList = new LinkedList<BasePanel>();

        [SerializeField]
        private Transform m_PanelRoot;

        #endregion

        #region 公共字段

        #endregion

        #region 属性

        public IReadOnlyDictionary<string, BasePanel> UIPanelDict => m_UIPanelDict;

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取或实例化面板
        /// </summary>
        /// <param name="_panelType"></param>
        /// <returns></returns>
        private BasePanel GetOrInstantiatePanel(string _panelType)
        {
            if (!TryGetPanel(_panelType, out BasePanel panel))
            {
                string path = m_Config.GetUIPanelPath(_panelType);

                if (string.IsNullOrEmpty(path))
                {
                    Debug.LogError($"无法读取Panel:  {_panelType.ToString()}");
                    return null;
                }

                panel = Instantiate(Resources.Load<GameObject>(path)).GetComponent<BasePanel>();
                panel.gameObject.name = _panelType.ToString();
                panel.transform.SetParent(m_PanelRoot, false);

                m_UIPanelDict.Add(_panelType, panel);

                panel.Initialize(_panelType, m_Config.UICamera);

                EventCenter<UIPanelEventCode>.Broadcast(UIPanelEventCode.InstantiatePanel, panel);
            }

            return panel;
        }

        #endregion

        #region 公共方法

        public void Initialize(IUIPanelConfig _config)
        {
            m_Config = _config;
        }

        #region 基础方法

        /// <summary>
        /// 打开面板,入栈
        /// </summary>
        /// <param name="_panelType"></param>
        /// <param name="_stopTimeScale">是否暂停游戏</param>
        /// <returns></returns>
        public async UniTask<BasePanel> PushPanelAsync(string _panelType, bool _await = true)
        {
            BasePanel pushedPanel = GetOrInstantiatePanel(_panelType);

            if (pushedPanel == null)
            {
                Debug.LogError($"Panel不存在:  {_panelType}");
                return null;
            }

            if (m_PanelList.Contains(pushedPanel))
            {
                Debug.LogWarning($"试图重复显示同一个Panel:  {_panelType}");
                return pushedPanel;
            }

            if (pushedPanel.PanelFormType == UIPanelFormType.Resident)
            {
                m_PanelList.AddFirst(pushedPanel);
            }
            else
            {
                if (m_PanelList.Count > 0 && m_PanelList.Last.Value.PanelFormType == UIPanelFormType.Popup)
                {
                    BasePanel lastPanel = m_PanelList.Last.Value;

                    //Debug.Log("暂停面板: " + lastPanel.name);

                    if (_await)
                        await lastPanel.PauseProcess();
                    else
                        lastPanel.PauseProcess().Forget();
                }
                else
                {
                    EventCenter<UIPanelEventCode>.Broadcast(UIPanelEventCode.PushFirstPopupPanel, pushedPanel);
                }

                m_PanelList.AddLast(pushedPanel);
            }

            //Debug.Log("打开面板: " + newPanel.name);

            if (_await)
                await pushedPanel.EnterProcess();
            else
                pushedPanel.EnterProcess().Forget();

            return pushedPanel;
        }

        /// <summary>
        /// 打开面板,入栈
        /// </summary>
        /// <param name="_panelType"></param>
        /// <param name="_stopTimeScale"></param>
        //public BasePanel PushPanel(string _panelType, bool _stopTimeScale = false)
        public BasePanel PushPanel(string _panelType)
        {
            PushPanelAsync(_panelType, false).Forget();

            return GetPanel(_panelType);
        }

        /// <summary>
        /// 打开面板,入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_panelType"></param>
        /// <param name="_stopTimeScale"></param>
        /// <returns></returns>
        public async UniTask<T> PushPanelAsync<T>(string _panelType, bool _await = true) where T : BasePanel
        {
            BasePanel panel = await PushPanelAsync(_panelType, _await);
            return panel != null && panel is T ?
                   panel as T :
                   null;
        }

        /// <summary>
        /// 打开面板,入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_panelType"></param>
        /// <param name="_stopTimeScale"></param>
        /// <returns></returns>
        public T PushPanel<T>(string _panelType) where T : BasePanel
        {
            PushPanelAsync(_panelType, false).Forget();

            return GetPanel<T>(_panelType);
        }

        /// <summary>
        /// 关闭顶层弹窗式面板,出栈
        /// </summary>
        public async UniTask PopPopupPanelAsync(bool _await = true)
        {
            if (m_PanelList.Count == 0 || m_PanelList.Last.Value.PanelFormType == UIPanelFormType.Resident)
                return;

            //Debug.Log("关闭面板: " + m_PanelList.Last.Value.name);

            BasePanel panel = m_PanelList.Last.Value;

            if (!panel.Closable)
                return;

            m_PanelList.RemoveLast();

            if (_await)
                await panel.ExitProcess();
            else
                panel.ExitProcess().Forget();

            if (panel.DestroyWhenExit)
                DestroyPanel(panel);

            if (m_PanelList.Count > 0 && m_PanelList.Last.Value.PanelFormType == UIPanelFormType.Popup)
            {
                BasePanel lastPanel = m_PanelList.Last.Value;

                //Debug.Log("恢复面板: " + lastPanel.name);

                if (_await)
                    await lastPanel.ResumeProcess();
                else
                    lastPanel.ResumeProcess().Forget();
            }
            else
            {
                EventCenter<UIPanelEventCode>.Broadcast(UIPanelEventCode.PopLastPopupPanel, panel);
            }
        }

        /// <summary>
        /// 关闭顶层弹窗式面板,出栈
        /// </summary>
        public void PopPopupPanel()
        {
            PopPopupPanelAsync(false).Forget();
        }

        /// <summary>
        /// 关闭指定弹出式面板及其上的所有面板,出栈
        /// </summary>
        /// <param name="_panelType"></param>
        public async UniTask PopSpecifiedPopupPanelAsync(string _panelType, bool _await = true)
        {
            if (m_UIPanelDict.TryGetValue(_panelType, out BasePanel panel) && panel.PanelFormType == UIPanelFormType.Popup)
            {
                //Debug.Log("关闭面板: " + panel.name);

                if (_await)
                {
                    while (m_PanelList.Contains(panel))
                    {
                        await PopPopupPanelAsync();
                    }
                }
                else
                {
                    while (m_PanelList.Contains(panel))
                    {
                        PopPopupPanelAsync(false).Forget();
                    }
                }
            }
        }

        public void PopSpecifiedPopupPanel(string _panelType)
        {
            PopSpecifiedPopupPanelAsync(_panelType, false).Forget();
        }

        /// <summary>
        /// 关闭指定常驻式面板,出栈
        /// </summary>
        /// <param name="_panelType"></param>
        public async UniTask PopSpecifiedResidentPanelAsync(string _panelType, bool _await = true)
        {
            if (m_UIPanelDict.TryGetValue(_panelType, out BasePanel panel) &&
                m_PanelList.Contains(panel) &&
                panel.PanelFormType == UIPanelFormType.Resident &&
                panel.Closable)
            {
                //Debug.Log("关闭面板: " + panel.name);

                if (_await)
                    await panel.ExitProcess();
                else
                    panel.ExitProcess().Forget();

                m_PanelList.Remove(panel);

                if (panel.DestroyWhenExit)
                    DestroyPanel(panel);
            }
        }

        /// <summary>
        /// 关闭指定常驻式面板,出栈
        /// </summary>
        /// <param name="_panelType"></param>
        public void PopSpecifiedResidentPanel(string _panelType)
        {
            PopSpecifiedResidentPanelAsync(_panelType, false).Forget();
        }

        /// <summary>
        /// 开启或关闭面板
        /// </summary>
        /// <param name="_panelType"></param>
        /// <returns></returns>
        public async UniTask<(BasePanel, bool)> SwitchPanelAsync(string _panelType, bool _await = true)
        {
            BasePanel panel = GetPanel(_panelType);

            if (panel != null && m_PanelList.Contains(panel))
            {
                if (panel.PanelFormType == UIPanelFormType.Resident)
                {
                    if (_await)
                        await PopSpecifiedResidentPanelAsync(_panelType);
                    else
                        PopSpecifiedResidentPanelAsync(_panelType).Forget();
                }
                else
                {
                    //如果指定UI上层还有其他UI则全部关闭
                    if (_await)
                        await PopSpecifiedPopupPanelAsync(_panelType);
                    else
                        PopSpecifiedPopupPanelAsync(_panelType).Forget();
                }

                return (panel, false);
            }
            else
            {
                return (await PushPanelAsync(_panelType, _await), true);
            }
        }

        public (BasePanel panel, bool isEnabled) SwitchPanel(string _panelType, bool _stopTimeScale = false)
        {
            SwitchPanelAsync(_panelType, false).Forget();

            BasePanel panel = GetPanel(_panelType);

            return (panel, m_PanelList.Contains(panel));
        }

        ///// <summary>
        ///// 暂停最上层面板
        ///// </summary>
        //public async UniTask PauseLastPanel(bool _await = true)
        //{
        //    if (m_PanelList.Count > 0 && m_PanelList.Last.Value.PanelFormType == UIPanelFormType.Popup)
        //    {
        //        BasePanel panel = m_PanelList.Last.Value;

        //        Debug.Log("暂停面板: " + panel.name);

        //        if (_await)
        //            await panel.PauseProcess();
        //        else
        //            panel.PauseProcess().Forget();
        //    }
        //}

        ///// <summary>
        ///// 恢复最上层面板
        ///// </summary>
        //public async UniTask ResumeLastPanel(bool _await = true)
        //{
        //    if (m_PanelList.Count > 0 && m_PanelList.Last.Value.PanelFormType == UIPanelFormType.Popup)
        //    {
        //        BasePanel panel = m_PanelList.Last.Value;

        //        Debug.Log("恢复面板: " + panel.name);

        //        if (_await)
        //            await panel.ResumeProcess();
        //        else
        //            panel.ResumeProcess().Forget();
        //    }
        //}

        /// <summary>
        /// 获取指定类型的面板
        /// </summary>
        /// <param name="_panelType"></param>
        /// <returns></returns>
        public BasePanel GetPanel(string _panelType)
        {
            //if (m_UIPanelDict.TryGetValue(_uiPanelType, out BasePanel panel) && m_PanelList.Contains(panel))
            //return m_PanelList.Find(panel).Value;
            if (m_UIPanelDict.TryGetValue(_panelType, out BasePanel panel))
                return panel;

            return null;
        }

        /// <summary>
        /// 获取指定类型的面板
        /// </summary>
        /// <param name="_panelType"></param>
        /// <returns></returns>
        public T GetPanel<T>(string _panelType) where T : BasePanel
        {
            BasePanel panel = GetPanel(_panelType);
            return panel != null ? panel as T : null;
        }

        /// <summary>
        /// 尝试获取指定类型的面板
        /// </summary>
        /// <param name="_panelType"></param>
        /// <param name="_panel"></param>
        /// <returns></returns>
        public bool TryGetPanel(string _panelType, out BasePanel _panel)
        {
            _panel = GetPanel(_panelType);
            return _panel != null;
        }

        /// <summary>
        /// 尝试获取指定类型的面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_panelType"></param>
        /// <param name="_panel"></param>
        /// <returns></returns>
        public bool TryGetPanel<T>(string _panelType, out T _panel) where T : BasePanel
        {
            _panel = (T)GetPanel(_panelType);
            return _panel != null;
        }

        /// <summary>
        /// 获取或打开指定面板
        /// </summary>
        /// <param name="_panelType"></param>
        /// <returns></returns>
        public async UniTask<BasePanel> GetOrPushPanelAsync(string _panelType, bool _await = true)
        {
            BasePanel panel = GetPanel(_panelType);

            return panel != null && m_PanelList.Contains(panel) ?
                   panel :
                   await PushPanelAsync(_panelType, _await);
        }

        /// <summary>
        /// 获取或打开指定面板
        /// </summary>
        /// <param name="_panelType"></param>
        /// <param name="_stopTimeScale"></param>
        /// <returns></returns>
        public BasePanel GetOrPushPanel(string _panelType)
        {
            BasePanel panel = GetPanel(_panelType);

            return panel != null && m_PanelList.Contains(panel) ?
                   panel :
                   PushPanel(_panelType);
        }

        /// <summary>
        /// 获取或打开指定面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_panelType"></param>
        /// <returns></returns>
        public async UniTask<T> GetOrPushPanelAsync<T>(string _panelType, bool _await = true) where T : BasePanel
        {
            return await GetOrPushPanelAsync(_panelType, _await) as T;
        }

        /// <summary>
        /// 获取或打开指定面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_panelType"></param>
        /// <param name="_stopTimeScale"></param>
        /// <returns></returns>
        public T GetOrPushPanel<T>(string _panelType) where T : BasePanel
        {
            return GetOrPushPanel(_panelType) as T;
        }

        /// <summary>
        /// 获取顶层弹出式面板
        /// </summary>
        /// <returns></returns>
        public BasePanel GetTopPanel()
        {
            if (m_PanelList.Count > 0 && m_PanelList.Last.Value.PanelFormType == UIPanelFormType.Popup)
                return m_PanelList.Last.Value;

            return null;
        }

        /// <summary>
        /// 关闭所有弹窗面板
        /// </summary>
        public async UniTask PopAllPopupPanelAsync(bool _await = true)
        {
            while (m_PanelList.Count > 0 && m_PanelList.Last.Value.PanelFormType == UIPanelFormType.Popup)
            {
                if (_await)
                    await PopPopupPanelAsync();
                else
                    PopPopupPanelAsync().Forget();
            }
        }

        public void PopAllPopupPanel()
        {
            PopAllPopupPanelAsync(false).Forget();
        }

        /// <summary>
        /// 关闭所有常驻面板
        /// </summary>
        public async UniTask PopAllResidentPanelAsync(bool _await = true)
        {
            while (m_PanelList.Count > 0 && m_PanelList.First.Value.PanelFormType == UIPanelFormType.Resident)
            {
                if (_await)
                    await m_PanelList.First.Value.ExitProcess();
                else
                    m_PanelList.First.Value.ExitProcess().Forget();

                m_PanelList.RemoveFirst();
            }
        }

        /// <summary>
        /// 关闭所有常驻面板
        /// </summary>
        public void PopAllResidentPanel()
        {
            PopAllResidentPanelAsync(false).Forget();
        }

        /// <summary>
        /// 关闭所有面板
        /// </summary>
        public async UniTask PopAllPanelAsync(bool _await = true)
        {
            await PopAllPopupPanelAsync(_await);
            await PopAllResidentPanelAsync(_await);
        }

        public void PopAllPanel()
        {
            PopAllPanelAsync(false).Forget();
        }

        /// <summary>
        /// 销毁面板
        /// </summary>
        /// <param name="_panel"></param>
        public void DestroyPanel(BasePanel _panel)
        {
            if (m_PanelList.Contains(_panel))
            {
                Debug.LogWarning($"不能销毁显示中的面板:  {_panel.name}");
                return;
            }

            m_UIPanelDict.Remove(_panel.UIPanelType);

            Destroy(_panel.gameObject);
        }

        /// <summary>
        /// 销毁面板
        /// </summary>
        /// <param name="_panelType"></param>
        public void DestroyPanel(string _panelType)
        {
            foreach (BasePanel panel in m_PanelList)
            {
                if (panel.UIPanelType.Equals(_panelType))
                {
                    DestroyPanel(panel);
                    return;
                }
            }
        }

        #endregion

        #region 拓展方法

        ///// <summary>
        ///// 打开初始面板
        ///// </summary>
        //private void PushInitialPanel()
        //{
        //    foreach (UIPanelType uiPanelType in m_InitialPushPanelArray)
        //    {
        //        PushPanel(uiPanelType).Forget();
        //    }

        //    if (Application.isEditor || Debug.isDebugBuild)
        //        PushPanel(UIPanelType.DebugPanel).Forget();
        //}

        ///// <summary>
        ///// 弹出提示
        ///// </summary>
        ///// <param name="_content"></param>
        //public void Tip(string _content)
        //{
        //    TipsPanel panel = GetPanel<TipsPanel>(UIPanelType.TipsPanel);
        //    panel.AddTipSequence(_content);
        //    panel.transform.SetAsLastSibling();
        //}

        //public void Tip(uint _wordsID)
        //{
        //    //TODO:Tips
        //    Tip(_wordsID.ToLocalization());
        //}

        ///// <summary>
        ///// 弹出对话框
        ///// </summary>
        ///// <param name="_content"></param>
        //public void PushDialog(string _content)
        //{
        //    PushPanel<SayDialogPanel>(UIType.SayDialogPanel).SetContent(_content);
        //}

        ///// <summary>
        ///// 弹出对话框
        ///// </summary>
        ///// <param name="_contentList"></param>
        //public void PushDialog(List<string> _contentList)
        //{
        //    PushPanel<SayDialogPanel>(UIType.SayDialogPanel).SetContent(_contentList);
        //}

        /// <summary>
        /// 检查指定面板是否正在展示中
        /// </summary>
        /// <param name="_panel"></param>
        /// <returns></returns>
        public bool CheckIsShowing(BasePanel _panel)
        {
            switch (_panel.PanelFormType)
            {
                case UIPanelFormType.Resident:
                    return m_PanelList.Contains(_panel);
                case UIPanelFormType.Popup:
                    return m_PanelList.Last() == _panel;
                default:
                    return false;
            }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// UI面板事件枚举码
    /// </summary>
    public enum UIPanelEventCode
    {
        /// <summary>
        /// 实例化面板
        /// </summary>
        InstantiatePanel,
        /// <summary>
        /// 进入面板
        /// </summary>
        EnterPanel,
        /// <summary>
        /// 退出面板
        /// </summary>
        ExitPanel,
        /// <summary>
        /// 暂停面板
        /// </summary>
        PausePanel,
        /// <summary>
        /// 恢复面板
        /// </summary>
        ResumePanel,
        /// <summary>
        /// 打开首个弹窗式面板
        /// </summary>
        PushFirstPopupPanel,
        /// <summary>
        /// 关闭最后一个弹窗式面板
        /// </summary>
        PopLastPopupPanel,
    }
}
#endif