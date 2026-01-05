using OfflineFantasy.GameCraft.Utility;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Unit.Model
{
    /// <summary>
    /// 单位通用消耗属性
    /// </summary>
    public class UnitCommonConsumableAttribute : UnitCommonUniversalAttribute, IUnitConsumableAttribute
    {
        #region 保护字段

        [Newtonsoft.Json.JsonProperty(Required = Newtonsoft.Json.Required.Default)]
        protected float m_CurrentValue;

        #endregion

        #region 属性

        [Newtonsoft.Json.JsonIgnore]
        public virtual float CurrentValue
        {
            get => m_CurrentValue;
            set
            {
                m_CurrentValue = Mathf.Clamp(value, CurrentValueLowerLimit, CurrentValueUpperLimit);
                BroadcastAttributeChange();
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public virtual float CurrentValueUpperLimit => FinalValue;

        [Newtonsoft.Json.JsonIgnore]
        public virtual float CurrentValueLowerLimit => 0;

        [Newtonsoft.Json.JsonIgnore]
        public virtual float CurrentPercentage => CurrentValue / CurrentValueUpperLimit;

        [Newtonsoft.Json.JsonIgnore]
        public virtual int CurrentIntegerValue => CurrentValue.CeilToInt();

        #endregion

        #region 构造函数

        public UnitCommonConsumableAttribute() : base() { }

        public UnitCommonConsumableAttribute(IUnitInfo _ownerUnitInfo, string _attributeType, float _baseValue, float _currentValue) :
            base(_ownerUnitInfo, _attributeType, _baseValue)
        {
            m_CurrentValue = _currentValue;
        }

        #endregion
    }
}