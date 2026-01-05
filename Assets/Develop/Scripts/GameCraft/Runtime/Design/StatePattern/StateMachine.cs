using System.Collections.Generic;
using OfflineFantasy.GameCraft.Utility.Manager;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Design.FSM
{
    public class StateMachine<StateType>
    {
        #region 保护字段

        protected Dictionary<StateType, IState> m_StateDict = new Dictionary<StateType, IState>();

        protected StateType m_CurrentStateIndex;

        protected IState m_CurrentState;

        #endregion

        #region 属性

        public IReadOnlyDictionary<StateType, IState> StateDict => m_StateDict;

        public StateType CurrentStateIndex => m_CurrentStateIndex;

        public IState CurrentState => m_CurrentState;

        #endregion

        public StateMachine(bool _enableUpdate = false, bool _enableFixedUpdate = false, bool _enableLateUpdate = false)
        {
            if (_enableUpdate)
                UpdateManager.Instance.m_UpdateAction += Update;
            if (_enableFixedUpdate)
                UpdateManager.Instance.m_FixedUpdateAction += FixedUpdate;
            if (_enableUpdate)
                UpdateManager.Instance.m_LateUpdateAction += LateUpdate;
        }

        ~StateMachine()
        {
            if (UpdateManager.Instance != null)
            {
                UpdateManager.Instance.m_UpdateAction -= Update;
                UpdateManager.Instance.m_FixedUpdateAction -= FixedUpdate;
                UpdateManager.Instance.m_LateUpdateAction -= LateUpdate;
            }
        }

        #region 私有方法


        #endregion

        #region 公共方法

        /// <summary>
        /// 注册状态
        /// </summary>
        /// <param name="_stateType"></param>
        /// <param name="_state"></param>
        public virtual void RegisterState(StateType _stateType, IState _state)
        {
            m_StateDict.Add(_stateType, _state);
            _state.Initialize();
        }

        /// <summary>
        /// 注销状态
        /// </summary>
        /// <param name="_stateName"></param>
        public virtual void UnregisterState(StateType _stateType)
        {
            if (m_StateDict.TryGetValue(_stateType, out IState removingState) &&
                removingState == m_CurrentState)
            {
                Debug.LogError($"不能注销当前状态:  {_stateType}");
                return;
            }

            m_StateDict.Remove(_stateType);
        }

        /// <summary>
        /// 改变状态
        /// </summary>
        /// <param name="_stateType"></param>
        public virtual void ChangeState(StateType _stateType)
        {
            if (m_CurrentState != null)
                m_CurrentState.Exit();

            m_CurrentStateIndex = _stateType;

            if (m_StateDict.TryGetValue(_stateType, out m_CurrentState))
                m_CurrentState.Enter();
            else
                Debug.LogError($"状态不存在:  {_stateType}");
        }

        public virtual void Update()
        {
            if (m_CurrentState != null)
                m_CurrentState.Update();
        }

        public virtual void FixedUpdate()
        {
            if (m_CurrentState != null)
                m_CurrentState.FixedUpdate();
        }

        public virtual void LateUpdate()
        {
            if (m_CurrentState != null)
                m_CurrentState.LateUpdate();
        }

        #endregion
    }
}