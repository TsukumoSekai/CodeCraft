//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using FrameCraft.Extension;
//using NetNorth.Battle.Unit.Controller;
//using FrameCraft.Attributes;
//using NetNorth.Utility;
//using NetNorth.Battle.Unit.Interface;
//using NetNorth.Battle.Ability.Model;
//using FrameCraft.Utility.Interface;
//using FrameCraft.Utility;
//using static NetNorth.Utility.Enums;

//namespace NetNorth.Battle.Unit.Model
//{
//    /// <summary>
//    /// 单位属性,
//    /// TODO:深度拆分
//    /// TODO:泛型
//    /// TODO:面向接口
//    /// TODO:用字典存储各类型数值方便拓展，并且可以解决常规属性和消耗属性不统一的问题或者把ConsumableAttribute作为接口使用
//    /// </summary>
//    [Serializable]
//    public class UnitBaseAttribute : IUnitAttribute
//    {
//        #region 私有字段

//        
//        private UnitAttributeType m_AttributeType;

//        private BaseUnitInfo m_OwnerUnitInfo;

//        private BaseUnitEntity m_OwnerUnitEntity;

//        #endregion

//        #region 保护字段

//        
//        protected float m_SourceValue;

//        
//        protected float m_LevelValue;

//        
//        protected float m_EquipmentValue;

//        /// <summary>
//        /// 固定值Buff数值字典，加法
//        /// </summary>
//        
//        protected Dictionary<uint, float> m_ConstantBuffDict = new Dictionary<uint, float>();

//        /// <summary>
//        /// 百分比Buff数值字典，乘法
//        /// </summary>
//        
//        protected Dictionary<uint, float> m_PercentageBuffDict = new Dictionary<uint, float>();

//        #endregion

//        #region 属性

//        
//        public BaseUnitInfo OwnerUnitInfo => m_OwnerUnitInfo;

//        
//        public BaseUnitEntity OwnerUnitEntity => m_OwnerUnitEntity ??= UnitController.Instance.GetUnitEntity<BaseUnitEntity>(OwnerUnitInfo.GUID);

//        
//        public UnitAttributeType AttributeType => m_AttributeType;

//        /// <summary>
//        /// 源数值，单位初始属性
//        /// </summary>
//        
//        public virtual float SourceValue
//        {
//            set
//            {
//                m_SourceValue = value;

//                BroadcastAttributeChange();
//            }
//            get => m_SourceValue;
//        }

//        /// <summary>
//        /// 等级数值，由升级获得
//        /// </summary>
//        
//        public virtual float LevelValue
//        {
//            set
//            {
//                m_LevelValue = value;

//                BroadcastAttributeChange();
//            }
//            get => m_LevelValue;
//        }

//        /// <summary>
//        /// 装备数值，由装备获得
//        /// </summary>
//        
//        public virtual float EquipmentValue
//        {
//            set
//            {
//                m_EquipmentValue = value;

//                BroadcastAttributeChange();
//            }
//            get => m_EquipmentValue;
//        }

//        /// <summary>
//        /// 基础数值，源数值+等级数值，用于作为Buff计算基数
//        /// </summary>
//        
//        public virtual float BaseValue
//        {
//            get
//            {
//                return m_SourceValue + m_LevelValue;
//            }
//        }

//        /// <summary>
//        /// 最终数值，(基础数值 * Buff倍率 + Buff数值 - Debuff数值 + 装备数值) * Debuff倍率
//        /// </summary>
//        
//        public virtual float FinalValue
//        {
//            get
//            {
//                float finalValue = BaseValue;
//                float mulBuffValue = 1f;
//                float mulDebuffValue = 1f;

//                foreach (float perValue in m_PercentageBuffDict.Values)
//                {
//                    if (perValue >= 0f)
//                        mulBuffValue += perValue;
//                    else
//                        mulDebuffValue -= perValue;
//                }

//                //TODO:下限限制
//                mulDebuffValue = Mathf.Max(mulDebuffValue, 0.1f);

//                finalValue *= mulBuffValue;

//                foreach (float constantValue in m_ConstantBuffDict.Values)
//                {
//                    finalValue += constantValue;
//                }

//                finalValue += m_EquipmentValue;

//                finalValue *= mulDebuffValue;

//                return finalValue;
//            }
//        }

//        /// <summary>
//        /// 基础整数数值
//        /// </summary>
//        
//        public virtual int BaseIntegerValue => BaseValue.CeilToInt();

//        /// <summary>
//        /// 最终整数数值
//        /// </summary>
//        
//        public virtual int FinalIntegerValue => FinalValue.CeilToInt();

//        #endregion

//        #region 构造方法

//        public UnitBaseAttribute() { }

//        public UnitBaseAttribute(BaseUnitInfo _unitInfo, UnitAttributeType _attributeType, float _sourceValue = 0)
//        {
//            m_AttributeType = _attributeType;

//            m_SourceValue = _sourceValue;
//        }

//        #endregion

//        #region 保护方法

//        protected void BroadcastAttributeChange()
//        {
//            if (m_OwnerUnitInfo != null && m_OwnerUnitInfo.m_OnAttributeChange != null)
//                m_OwnerUnitInfo.m_OnAttributeChange(this);
//        }

//        #endregion

//        #region 公共方法

//        /// <summary>
//        /// 初始化，需要在UnitInfo加载完成后调用
//        /// </summary>
//        public virtual void Initialize(BaseUnitInfo _ownerUnitInfo)
//        {
//            m_OwnerUnitInfo = _ownerUnitInfo;
//        }

//        /// <summary>
//        /// 刷新
//        /// </summary>
//        public virtual void Refresh() { }

//        /// <summary>
//        /// 增加或替换Buff数值
//        /// </summary>
//        /// <param name="_valueType"></param>
//        /// <param name="_guid"></param>
//        /// <param name="_value"></param>
//        public void AddOrReplaceBuffValue(ValueType _valueType, uint _guid, float _value)
//        {
//            if (_valueType == ValueType.Constant)
//                m_ConstantBuffDict.AddOrReplace(_guid, _value);
//            else
//                m_PercentageBuffDict.AddOrReplace(_guid, _value);
//        }

//        /// <summary>
//        /// 移除Buff数值
//        /// </summary>
//        /// <param name="_valueType"></param>
//        /// <param name="_guid"></param>
//        public void RemoveBuffValue(ValueType _valueType, uint _guid)
//        {
//            if (_valueType == ValueType.Constant)
//                m_ConstantBuffDict.Remove(_guid);
//            else
//                m_PercentageBuffDict.Remove(_guid);
//        }

//        #endregion
//    }

//    /// <summary>
//    /// 单位属性类型
//    /// </summary>
//    public enum UnitAttributeType
//    {
//        #region 一级属性

//        /// <summary>
//        /// 生命值
//        /// </summary>
//        [I18n("生命值")]
//        HP = 100,
//        /// <summary>
//        /// 魔法值
//        /// </summary>
//        [I18n("法力值")]
//        MP,
//        /// <summary>
//        /// 物理攻击
//        /// </summary>
//        [I18n("物理攻击")]
//        PhysicalAttack,
//        /// <summary>
//        /// 魔法攻击
//        /// </summary>
//        [I18n("魔法攻击")]
//        MagicAttack,
//        /// <summary>
//        /// 物理防御
//        /// </summary>
//        [I18n("物理防御")]
//        PhysicalDefence,
//        /// <summary>
//        /// 魔法防御
//        /// </summary>
//        [I18n("魔法防御")]
//        MagicDefence,

//        #endregion

//        #region 二级属性

//        /// <summary>
//        /// 护甲穿透
//        /// </summary>
//        [I18n("护甲穿透")]
//        ArmorPenetration = 200,
//        /// <summary>
//        /// 法术穿透
//        /// </summary>
//        [I18n("法术穿透")]
//        SpellPenetration,
//        /// <summary>
//        /// 暴击率
//        /// </summary>
//        [I18n("暴击率")]
//        CriticalChance,
//        /// <summary>
//        /// 暴击伤害
//        /// </summary>
//        [I18n("暴击伤害")]
//        CriticalDamage,
//        /// <summary>
//        /// 生命值再生
//        /// </summary>
//        [I18n("生命值再生")]
//        HPRegeneration,
//        /// <summary>
//        /// 法力值再生
//        /// </summary>
//        [I18n("法力值再生")]
//        MPRegeneration,
//        /// <summary>
//        /// 冷却
//        /// </summary>
//        [I18n("冷却")]
//        Cooldown,
//        /// <summary>
//        /// 攻击速度
//        /// </summary>
//        [I18n("攻击速度")]
//        AttackSpeed,
//        /// <summary>
//        /// 增伤
//        /// </summary>
//        [I18n("增伤")]
//        AdditionalDamage,
//        /// <summary>
//        /// 移动速度
//        /// </summary>
//        [I18n("移动速度")]
//        MoveSpeed,

//        #endregion

//        #region 通用属性

//        /// <summary>
//        /// 等级
//        /// </summary>
//        [I18n("等级")]
//        Level = 300,

//        #endregion

//        #region 玩家属性

//        /// <summary>
//        /// 经验
//        /// </summary>
//        [I18n("经验")]
//        Exp = 400,

//        #endregion

//        #region 宠物属性

//        /// <summary>
//        /// 成长值
//        /// </summary>
//        [I18n("成长值")]
//        Growth = 500,
//        /// <summary>
//        /// 饱食度
//        /// </summary>
//        [I18n("饱食度")]
//        Satiety = 501,

//        #endregion
//    }

//    /// <summary>
//    /// 数值类型
//    /// </summary>
//    public enum ValueType
//    {
//        /// <summary>
//        /// 固定值
//        /// </summary>
//        [I18n("固定值")]
//        Constant,
//        /// <summary>
//        /// 百分比值
//        /// </summary>
//        [I18n("百分比值")]
//        Percentage,
//    }

//    /// <summary>
//    /// 属性数据
//    /// </summary>
//    [System.Serializable]
//    public class AttributeData : ISerializable<AttributeData>
//    {
//        /// <summary>
//        /// 属性类型
//        /// </summary>
//        public UnitAttributeType m_AttributeType;

//        /// <summary>
//        /// 数值类型
//        /// </summary>
//        public ValueType m_ValueType;

//        /// <summary>
//        /// 数值
//        /// </summary>
//        public float m_Value;

//        public string Serialize(int _splitSymbolIndex = 0)
//        {
//            return $"{m_AttributeType}{SerializationUtility.m_SeparatorArray[_splitSymbolIndex]}{m_ValueType}{SerializationUtility.m_SeparatorArray[_splitSymbolIndex]}{m_Value}";
//        }

//        public bool Deserialize(string _context, out AttributeData _object, int _splitSymbolIndex = 0)
//        {
//            string[] contextArray = _context.Split(SerializationUtility.m_SeparatorArray[_splitSymbolIndex]);

//            switch (contextArray.Length)
//            {
//                case 2:
//                    m_AttributeType = (UnitAttributeType)int.Parse(contextArray[0]);
//                    m_Value = float.Parse(contextArray[1]);

//                    _object = this;
//                    return true;

//                case 3:
//                    m_AttributeType = (UnitAttributeType)int.Parse(contextArray[0]);
//                    m_ValueType = (ValueType)int.Parse(contextArray[1]);
//                    m_Value = float.Parse(contextArray[2]);

//                    _object = this;
//                    return true;

//                default:
//                    DebugCraft<LogModuleType>.LogError($"参数数量错误:  {_context}", LogModuleType.Debug);

//                    _object = null;
//                    return false;
//            }
//        }

//        public AttributeData() { }

//        public AttributeData(UnitAttributeType _attributeType, ValueType _valueType, float _value)
//        {
//            m_AttributeType = _attributeType;
//            m_ValueType = _valueType;
//            m_Value = _value;
//        }
//    }
//}