#if ENABLE_UNITASK
using Cysharp.Threading.Tasks;
using OfflineFantasy.GameCraft.Utility;
using OfflineFantasy.GameCraft.Utility.Event;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace OfflineFantasy.GameCraft.UI
{
    /// <summary>
    /// UI面板形式类型
    /// </summary>
    public enum UIPanelFormType
    {
        /// <summary>
        /// 常驻面板
        /// </summary>
        Resident,
        /// <summary>
        /// 弹窗面板
        /// </summary>
        Popup,
    }

    [System.Serializable]
    public class UIPanelEvent : UnityEvent { }

    /// <summary>
    /// 面板基类
    /// TODO:BasePanel类太过具体，应抽象出接口IPanel
    /// </summary>
    public abstract class BasePanel : MonoBehaviour, IPanel
    {
        //private static BasePanel m_Instance;

        //public static BasePanel Instance
        //{
        //    get
        //    {
        //        if (m_Instance == null)
        //            DebugCraft<LogModuleType>.LogError($"该面板未实例化", LogModuleType.Debug);

        //        return m_Instance;
        //    }
        //}

        #region 私有字段

        //由PanelFieldsClassGenerator.BoundFields绑定
        [SerializeField, HideInInspector]
        private Canvas m_Canvas;

        [SerializeField, HideInInspector]
        private CanvasGroup m_CanvasGroup;

        [SerializeField, HideInInspector]
        private CanvasScaler m_CanvasScaler;

        [SerializeField, HideInInspector]
        private GraphicRaycaster m_GraphicRaycaster;

        [SerializeField, Header("面板形式类型")]
        private UIPanelFormType m_PanelFormType = UIPanelFormType.Popup;

        [SerializeField, Header("退出时直接销毁")]
        private bool m_DestroyWhenExit = false;

        [SerializeField, Header("切换相机")]
        private bool m_SwitchCamera;

        #endregion

        #region 保护字段

        //TODO:这些配置应该走表

        /// <summary>
        /// 面板类型
        /// </summary>
        protected string m_UIPanelType;

        /// <summary>
        /// 是否启用字典
        /// </summary>
        [SerializeField, Header("是否启用字典(所有游戏物体必须不重名)")]
        protected bool m_UseDictionary = true;

        /// <summary>
        /// 是否可以手动关闭该面板
        /// </summary>
        [SerializeField, Header("是否可以手动关闭该面板")]
        protected bool m_EnableManualExit = true;

        [SerializeField, Header("进入时渐入")]
        protected bool m_FadeInWhenEnter = false;

        [SerializeField, Header("恢复时渐入")]
        protected bool m_FadeInWhenResume = false;

        [SerializeField, Header("渐入时长")]
        protected float m_FadeInDuration = 0.2f;

        [SerializeField, Header("退出时渐出")]
        protected bool m_FadeOutWhenExit = false;

        [SerializeField, Header("暂停时渐出")]
        protected bool m_FadeOutWhenPause = false;

        [SerializeField, Header("渐出时长")]
        protected float m_FadeOutDuration = 0.2f;

        [SerializeField]
        protected UIPanelEvent m_OnEnterPanelEvent = new UIPanelEvent();

        [SerializeField]
        protected UIPanelEvent m_OnExitPanelEvent = new UIPanelEvent();

        [SerializeField]
        protected UIPanelEvent m_OnPausePanelEvent = new UIPanelEvent();

        [SerializeField]
        protected UIPanelEvent m_OnResumePanelEvent = new UIPanelEvent();

        #endregion

        #region 属性

        public Canvas Canvas => m_Canvas;

        public CanvasGroup CanvasGroup => m_CanvasGroup;

        public CanvasScaler CanvasScaler => m_CanvasScaler;

        public GraphicRaycaster GraphicRaycaster => m_GraphicRaycaster;

        /// <summary>
        /// 面板形式类型
        /// </summary>
        public UIPanelFormType PanelFormType => m_PanelFormType;

        /// <summary>
        /// 面板类型
        /// </summary>
        public string UIPanelType => m_UIPanelType;

        /// <summary>
        /// 退出时直接销毁
        /// </summary>
        public bool DestroyWhenExit => m_DestroyWhenExit;

        public bool IsShowing { get; private set; }

        public virtual bool Closable => true;

        public bool FadeInWhenEnter { get => m_FadeInWhenEnter; set => m_FadeInWhenEnter = value; }

        public bool FadeInWhenResume { get => m_FadeInWhenResume; set => m_FadeInWhenResume = value; }

        public float FadeInDuration { get => m_FadeInDuration; set => m_FadeInDuration = value; }

        public bool FadeOutWhenExit { get => m_FadeOutWhenExit; set => m_FadeOutWhenExit = value; }

        public bool FadeOutWhenPause { get => m_FadeOutWhenPause; set => m_FadeOutWhenPause = value; }

        public float FadeOutDuration { get => m_FadeOutDuration; set => m_FadeOutDuration = value; }

        public UIPanelEvent OnEnterPanelEvent => m_OnEnterPanelEvent;

        public UIPanelEvent OnExitPanelEvent => m_OnExitPanelEvent;

        public UIPanelEvent OnPausePanelEvent => m_OnPausePanelEvent;

        public UIPanelEvent OnResumePanelEvent => m_OnResumePanelEvent;

        #endregion

        #region 生命周期

        //NOTE:Prepare用于预加载，Doing用于动画表现，Complete用于完成时

        /// <summary>
        /// 进入该面板,该面板被弹出(Push)时调用
        /// </summary>
        public async UniTask EnterProcess()
        {
            PrepareEnter();

            await Entering();

            CompleteEnter();
        }

        /// <summary>
        /// 退出该面板,该面板被关闭(Pop)时调用
        /// </summary>
        public async UniTask ExitProcess()
        {
            PrepareExit();

            await Exiting();

            CompleteExit();
        }

        /// <summary>
        /// 恢复该面板,上层面板关闭后调用
        /// </summary>
        public async UniTask ResumeProcess()
        {
            PrepareResume();

            await Resuming();

            CompleteResume();
        }

        /// <summary>
        /// 暂停该面板,该面板被另一个弹窗式面板覆盖时调用
        /// </summary>
        public async UniTask PauseProcess()
        {
            PreparePause();

            await Pausing();

            CompletePause();
        }

        #endregion

        #region 私有方法

        #endregion

        #region 保护方法

        #region 进入(Enter)

        /// <summary>
        /// 准备进入
        /// </summary>
        protected virtual void PrepareEnter()
        {
            IsShowing = true;

            transform.SetAsLastSibling();

            EventCenter<GlobalEventCode>.AddListener(GlobalEventCode.ChangeLanguage, OnChangeLanguage);
        }

        /// <summary>
        /// 进入中
        /// </summary>
        /// <returns></returns>
        protected async virtual UniTask Entering()
        {
            if (m_FadeInWhenEnter)
                await CanvasGroup.FadeIn(m_FadeInDuration);
            else
                CanvasGroup.alpha = 1f;
        }

        /// <summary>
        /// 完成进入
        /// </summary>
        protected virtual void CompleteEnter()
        {
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;

            m_OnEnterPanelEvent.Invoke();
            EventCenter<UIPanelEventCode>.Broadcast(UIPanelEventCode.EnterPanel, this);
        }

        #endregion

        #region 退出(Exit)

        /// <summary>
        /// 准备退出
        /// </summary>
        protected virtual void PrepareExit()
        {
            IsShowing = false;

            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.interactable = false;

            EventCenter<GlobalEventCode>.RemoveListener(GlobalEventCode.ChangeLanguage, OnChangeLanguage);
        }

        /// <summary>
        /// 退出中
        /// </summary>
        /// <returns></returns>
        protected async virtual UniTask Exiting()
        {
            if (m_FadeOutWhenExit)
                await CanvasGroup.FadeOut(m_FadeOutDuration);
            else
                CanvasGroup.alpha = 0f;
        }

        /// <summary>
        /// 完成退出
        /// </summary>
        protected virtual void CompleteExit()
        {
            m_OnExitPanelEvent.Invoke();
            EventCenter<UIPanelEventCode>.Broadcast(UIPanelEventCode.ExitPanel, this);
        }

        #endregion

        #region 恢复(Resume)

        /// <summary>
        /// 准备恢复
        /// </summary>
        protected virtual void PrepareResume()
        {
        }

        /// <summary>
        /// 恢复中
        /// </summary>
        /// <returns></returns>
        protected async UniTask Resuming()
        {
            if (m_FadeInWhenResume)
                await CanvasGroup.FadeIn(m_FadeInDuration);
            else
                CanvasGroup.alpha = 1f;
        }

        /// <summary>
        /// 完成恢复
        /// </summary>
        protected virtual void CompleteResume()
        {
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;

            m_OnResumePanelEvent.Invoke();
            EventCenter<UIPanelEventCode>.Broadcast(UIPanelEventCode.ResumePanel, this);
        }

        #endregion

        #region 暂停(Pause)

        /// <summary>
        /// 准备暂停
        /// </summary>
        protected virtual void PreparePause()
        {
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// 暂停中
        /// </summary>
        /// <returns></returns>
        protected async UniTask Pausing()
        {
            if (m_FadeOutWhenPause)
                await CanvasGroup.FadeOut(m_FadeOutDuration);
            else
                CanvasGroup.alpha = 0f;
        }

        /// <summary>
        /// 完成暂停
        /// </summary>
        protected virtual void CompletePause()
        {
            m_OnPausePanelEvent?.Invoke();
            EventCenter<UIPanelEventCode>.Broadcast(UIPanelEventCode.PausePanel, this);
        }

        #endregion

        /// <summary>
        /// 接收取消键
        /// </summary>
        protected virtual void OnUICancel()
        {
            if (m_EnableManualExit && UIPanelManager.Instance.GetTopPanel() == this)
                Close();
        }

        /// <summary>
        /// 当切换语言时，用于刷新文本
        /// </summary>
        protected virtual void OnChangeLanguage() { }

        #endregion

        #region 公共方法

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_uiPanelType"></param>
        public virtual void Initialize(string _uiPanelType, Camera _uiCamera)
        {
            //m_Instance = this;
            m_UIPanelType = _uiPanelType;
            m_Canvas.worldCamera = _uiCamera;
        }

        /// <summary>
        /// 显示/隐藏面板
        /// </summary>
        /// <param name="_visible"></param>
        public void SetVisible(bool _visible)
        {
            if (_visible)
            {
                CanvasGroup.alpha = 1f;
                CanvasGroup.interactable = true;
                CanvasGroup.blocksRaycasts = true;
            }
            else
            {
                CanvasGroup.alpha = 0f;
                CanvasGroup.interactable = false;
                CanvasGroup.blocksRaycasts = false;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void Close()
        {
            switch (PanelFormType)
            {
                case UIPanelFormType.Resident:
                    UIPanelManager.Instance.PopSpecifiedResidentPanel(m_UIPanelType);
                    break;
                case UIPanelFormType.Popup:
                    UIPanelManager.Instance.PopSpecifiedPopupPanel(m_UIPanelType);
                    break;
            }
        }

        #endregion
    }
}
#endif