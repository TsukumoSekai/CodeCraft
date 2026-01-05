using System;
using System.Collections;
using System.Collections.Generic;
using OfflineFantasy.GameCraft.Design;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Utility.Manager
{
    /// <summary>
    /// 协程管理器
    /// </summary>
    public class CoroutineManager : UnitySingleton<CoroutineManager>
    {
        #region 枚举

        /// <summary>
        /// 取消类型
        /// </summary>
        [Flags]
        public enum CancelType
        {
            /// <summary>
            /// 不会自动取消
            /// </summary>
            Null = 0,
            /// <summary>
            /// 改变场景时取消
            /// </summary>
            OnChangeScene = 1,
        }

        #endregion

        #region 私有字段

        /// <summary>
        /// 协程字典
        /// </summary>
        private Dictionary<CancelType, List<Coroutine>> m_CoroutineDict;

        #endregion

        #region 私有方法

        //private void OnDestroy()
        //{
        //    SceneManager.sceneUnloaded -= CancelDelayedInvoke;
        //}

        //private void CancelDelayedInvoke(Scene _scene)
        //{
        //    CancelDelayedInvokeWithType(CancelType.OnChangeScene);

        private void AddCoroutine(Coroutine _coroutine, CancelType _cancelType)
        {
            foreach (KeyValuePair<CancelType, List<Coroutine>> kv in Instance.m_CoroutineDict)
            {
                if (_cancelType.HasFlag(kv.Key))
                    kv.Value.Add(_coroutine);
            }
        }

        private IEnumerator ICoroutine(float _delay, Action _callback, bool _scaledTime, int _repeatCount)
        {
            for (int i = 0; i < _repeatCount; i++)
            {
                if (_scaledTime)
                    yield return new WaitForSecondsRealtime(_delay);
                else
                    yield return new WaitForSeconds(_delay);
                _callback?.Invoke();
            }
        }

        private IEnumerator ICoroutine<T>(float _delay, Action<T> _callback, T _arg, bool _scaledTime, int _repeatCount)
        {
            for (int i = 0; i < _repeatCount; i++)
            {
                if (_scaledTime)
                    yield return new WaitForSecondsRealtime(_delay);
                else
                    yield return new WaitForSeconds(_delay);
                _callback?.Invoke(_arg);
            }
        }

        private IEnumerator ICoroutine<T1, T2>(float _delay, Action<T1, T2> _callback, T1 _arg1, T2 _arg2, bool _scaledTime, int _repeatCount)
        {
            for (int i = 0; i < _repeatCount; i++)
            {
                if (_scaledTime)
                    yield return new WaitForSecondsRealtime(_delay);
                else
                    yield return new WaitForSeconds(_delay);
                _callback?.Invoke(_arg1, _arg2);
            }
        }

        private IEnumerator ICoroutine<T1, T2, T3>(float _delay, Action<T1, T2, T3> _callback, T1 _arg1, T2 _arg2, T3 _arg3, bool _scaledTime, int _repeatCount)
        {
            for (int i = 0; i < _repeatCount; i++)
            {
                if (_scaledTime)
                    yield return new WaitForSecondsRealtime(_delay);
                else
                    yield return new WaitForSeconds(_delay);
                _callback?.Invoke(_arg1, _arg2, _arg3);
            }
        }

        private IEnumerator ICoroutine<T1, T2, T3, T4>(float _delay, Action<T1, T2, T3, T4> _callback, T1 _arg1, T2 _arg2, T3 _arg3, T4 _arg4, bool _scaledTime, int _repeatCount)
        {
            for (int i = 0; i < _repeatCount; i++)
            {
                if (_scaledTime)
                    yield return new WaitForSecondsRealtime(_delay);
                else
                    yield return new WaitForSeconds(_delay);
                _callback?.Invoke(_arg1, _arg2, _arg3, _arg4);
            }
        }

        #endregion

        #region 保护方法

        protected override void Awake()
        {
            base.Awake();

            m_CoroutineDict = new Dictionary<CancelType, List<Coroutine>>();

            foreach (CancelType flag in Enum.GetValues(typeof(CancelType)))
            {
                if (flag == CancelType.Null)
                    continue;

                m_CoroutineDict.Add(flag, new List<Coroutine>());
            }

            //SceneManager.sceneUnloaded += CancelDelayedInvoke;
        }

        //protected override void InitSingleton()
        //{
        //    Instance = this;
        //}

        #endregion

        #region 公共方法

        #region 单例管理协程

        /// <summary>
        /// 延迟执行
        /// </summary>
        /// <param name="_delay">延迟时长</param>
        /// <param name="_callback">方法</param>
        /// <param name="_cancelType">取消类型</param>
        /// <param name="_scaledTime">是否受时间缩放影响</param>
        /// <param name="_repeatCount">重复执行次数</param>
        /// <returns></returns>
        public Coroutine DelayedInvoke(float _delay, Action _callback, CancelType _cancelType = CancelType.OnChangeScene, bool _scaledTime = true, int _repeatCount = 1)
        {
            Coroutine coroutine = Instance.StartCoroutine(ICoroutine(_delay, _callback, _scaledTime, _repeatCount));
            AddCoroutine(coroutine, _cancelType);
            return coroutine;
        }

        public Coroutine DelayedInvoke<T>(float _delay, Action<T> _callback, T _arg, CancelType _cancelType = CancelType.OnChangeScene, bool _scaledTime = true, int _repeatCount = 1)
        {
            Coroutine coroutine = Instance.StartCoroutine(ICoroutine(_delay, _callback, _arg, _scaledTime, _repeatCount));
            AddCoroutine(coroutine, _cancelType);
            return coroutine;
        }

        public Coroutine DelayedInvoke<T1, T2>(float _delay, Action<T1, T2> _callback, T1 _arg1, T2 _arg2, CancelType _cancelType = CancelType.OnChangeScene, bool _scaledTime = true, int _repeatCount = 1)
        {
            Coroutine coroutine = Instance.StartCoroutine(ICoroutine(_delay, _callback, _arg1, _arg2, _scaledTime, _repeatCount));
            AddCoroutine(coroutine, _cancelType);
            return coroutine;
        }

        public Coroutine DelayedInvoke<T1, T2, T3>(float _delay, Action<T1, T2, T3> _callback, T1 _arg1, T2 _arg2, T3 _arg3, CancelType _cancelType = CancelType.OnChangeScene, bool _scaledTime = true, int _repeatCount = 1)
        {
            Coroutine coroutine = Instance.StartCoroutine(ICoroutine(_delay, _callback, _arg1, _arg2, _arg3, _scaledTime, _repeatCount));
            AddCoroutine(coroutine, _cancelType);
            return coroutine;
        }

        public Coroutine DelayedInvoke<T1, T2, T3, T4>(float _delay, Action<T1, T2, T3, T4> _callback, T1 _arg1, T2 _arg2, T3 _arg3, T4 _arg4, CancelType _cancelType = CancelType.OnChangeScene, bool _scaledTime = true, int _repeatCount = 1)
        {
            Coroutine coroutine = Instance.StartCoroutine(ICoroutine(_delay, _callback, _arg1, _arg2, _arg3, _arg4, _scaledTime, _repeatCount));
            AddCoroutine(coroutine, _cancelType);
            return coroutine;
        }

        #endregion

        #region 调用者管理协程

        public Coroutine DelayedInvoke(MonoBehaviour _carrier, float _delay, Action _callback, bool _scaledTime = true, int _repeatCount = 1)
        {
            return _carrier.StartCoroutine(ICoroutine(_delay, _callback, _scaledTime, _repeatCount));
        }

        public Coroutine DelayedInvoke<T>(MonoBehaviour _carrier, float _delay, Action<T> _callback, T _arg, bool _scaledTime = true, int _repeatCount = 1)
        {
            return _carrier.StartCoroutine(ICoroutine(_delay, _callback, _arg, _scaledTime, _repeatCount));
        }

        public Coroutine DelayedInvoke<T1, T2>(MonoBehaviour _carrier, float _delay, Action<T1, T2> _callback, T1 _arg1, T2 _arg2, bool _scaledTime = true, int _repeatCount = 1)
        {
            return _carrier.StartCoroutine(ICoroutine(_delay, _callback, _arg1, _arg2, _scaledTime, _repeatCount));
        }

        public Coroutine DelayedInvoke<T1, T2, T3>(MonoBehaviour _carrier, float _delay, Action<T1, T2, T3> _callback, T1 _arg1, T2 _arg2, T3 _arg3, bool _scaledTime = true, int _repeatCount = 1)
        {
            return _carrier.StartCoroutine(ICoroutine(_delay, _callback, _arg1, _arg2, _arg3, _scaledTime, _repeatCount));
        }

        public Coroutine DelayedInvoke<T1, T2, T3, T4>(MonoBehaviour _carrier, float _delay, Action<T1, T2, T3, T4> _callback, T1 _arg1, T2 _arg2, T3 _arg3, T4 _arg4, bool _scaledTime = true, int _repeatCount = 1)
        {
            return _carrier.StartCoroutine(ICoroutine(_delay, _callback, _arg1, _arg2, _arg3, _arg4, _scaledTime, _repeatCount));
        }

        #endregion

        /// <summary>
        /// 取消等待协程
        /// </summary>
        /// <param name="_coroutine"></param>
        public void CancelDelayedInvoke(Coroutine _coroutine)
        {
            if (_coroutine != null)
            {
                Instance.StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        ///// <summary>
        ///// 取消所有指定类型的等待协程
        ///// </summary>
        ///// <param name="_cancelType"></param>
        //public static void CancelDelayedInvokeWithType(CancelType _cancelType)
        //{
        //    foreach (CancelType flag in _cancelType.GetFlags())
        //    {
        //        if (flag == CancelType.Null)
        //            continue;

        //        foreach (Coroutine coroutine in m_CoroutineDict[flag])
        //        {
        //            if (coroutine != null)
        //                Instance.StopCoroutine(coroutine);
        //        }

        //        m_CoroutineDict[flag].Clear();
        //    }
        //}

        #endregion
    }
}