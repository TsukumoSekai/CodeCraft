//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using FrameCraft.Extension;
//using NetNorth.Battle.Unit.Interface;

//namespace NetNorth.Battle.Unit.Model
//{
//    /// <summary>
//    /// 单位消耗型属性
//    /// </summary>
//    [Serializable]
//    public class UnitBaseConsumableAttribute : UnitBaseAttribute, IUnitConsumableAttribute
//    {
//        /// <summary>
//        /// 当前数值
//        /// </summary>
//        
//        protected float m_CurrentValue;

//        /// <summary>
//        /// 当前数值
//        /// </summary>
//        
//        public virtual float CurrentValue
//        {
//            set
//            {
//                m_CurrentValue = Mathf.Clamp(value, CurrentValueLowerLimit, CurrentValueUpperLimit);

//                BroadcastAttributeChange();
//            }
//            get => m_CurrentValue;
//        }

//        /// <summary>
//        /// 当前数值上限
//        /// </summary>
//        
//        public virtual float CurrentValueUpperLimit => FinalValue;

//        /// <summary>
//        /// 当前数值下限
//        /// </summary>
//        
//        public virtual float CurrentValueLowerLimit => 0f;

//        /// <summary>
//        /// 当前数值百分比
//        /// </summary>
//        
//        public virtual float CurrentPercentage
//        {
//            set => CurrentValue = value * CurrentValueUpperLimit;
//            get => CurrentValue / CurrentValueUpperLimit;
//        }

//        /// <summary>
//        /// 当前整数数值
//        /// </summary>
//        
//        public virtual int CurrentIntegerValue => CurrentValue.CeilToInt();

//        #region 构造方法

//        public UnitBaseConsumableAttribute() : base() { }

//        public UnitBaseConsumableAttribute(BaseUnitInfo _unitInfo, UnitAttributeType _attributeType, float _sourceValue = 0f, float _currentValue = 0f) :
//                                           base(_unitInfo, _attributeType, _sourceValue)
//        {
//            m_CurrentValue = _currentValue;
//        }

//        #endregion

//        public void RestoresToUpperLimit()
//        {
//            CurrentValue = FinalValue;
//        }
//    }
//}