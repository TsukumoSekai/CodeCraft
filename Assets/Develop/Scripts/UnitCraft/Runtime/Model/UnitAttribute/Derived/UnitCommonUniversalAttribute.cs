using System;
using OfflineFantasy.GameCraft.Utility;

namespace OfflineFantasy.GameCraft.Unit.Model
{
    /// <summary>
    /// 单位通用常规属性
    /// </summary>
    public class UnitCommonUniversalAttribute : IUnitAttribute
    {
        #region 保护字段

        [Newtonsoft.Json.JsonProperty(Required = Newtonsoft.Json.Required.Default)]
        protected uint m_OwnerUnitInfoGUID;

        [Newtonsoft.Json.JsonProperty(Required = Newtonsoft.Json.Required.Default)]
        protected string m_AttributeType;

        [Newtonsoft.Json.JsonProperty(Required = Newtonsoft.Json.Required.Default)]
        protected float m_BaseValue;

        protected IUnitInfo m_OwnerUnitInfo;

        #endregion

        #region 公共字段

        [Newtonsoft.Json.JsonIgnore]
        public Action<IUnitAttribute> m_OnValueChangeEvent;

        #endregion

        #region 属性

        [Newtonsoft.Json.JsonIgnore]
        public virtual string AttributeType => m_AttributeType;

        [Newtonsoft.Json.JsonIgnore]
        //public virtual IUnitInfo OwnerUnitInfo => m_OwnerUnitInfo ??= UnitManager.Instance.UnitInfoDict[m_OwnerUnitInfoGUID];
        public virtual IUnitInfo OwnerUnitInfo => m_OwnerUnitInfo;

        [Newtonsoft.Json.JsonIgnore]
        public virtual float BaseValue
        {
            get => m_BaseValue;
            set
            {
                m_BaseValue = value;
                BroadcastAttributeChange();
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public virtual float FinalValue => m_BaseValue;

        [Newtonsoft.Json.JsonIgnore]
        public virtual int FinalIntegerValue => m_BaseValue.RoundToInt();

        #endregion

        #region 构造函数

        public UnitCommonUniversalAttribute() { }

        public UnitCommonUniversalAttribute(IUnitInfo _ownerUnitInfo, string _attributeType, float _baseValue = 0f)
        {
            m_OwnerUnitInfoGUID = _ownerUnitInfo.GUID;
            m_OwnerUnitInfo = _ownerUnitInfo;
            m_AttributeType = _attributeType;
            m_BaseValue = _baseValue;
        }

        #endregion

        #region 保护方法

        protected virtual void BroadcastAttributeChange()
        {
            m_OnValueChangeEvent?.Invoke(this);
            m_OwnerUnitInfo?.OnUnitAttributeChangeEvent?.Invoke(this);
        }

        #endregion
    }
}