using System;

namespace OfflineFantasy.GameCraft.Unit.Model
{
    public interface IUnitInfo
    {
        public uint GUID { get; }

        public Action<IUnitAttribute> OnUnitAttributeChangeEvent { get; set; }

        public IUnitAttribute GetUnitAttribute(string _unitAttributeType);
        public IUnitAttribute GetUnitUniversalAttribute(string _unitAttributeType);
        public IUnitConsumableAttribute GetUnitConsumableAttribute(string _unitAttributeType);
    }
}