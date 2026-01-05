namespace OfflineFantasy.GameCraft.Unit.Model
{
    /// <summary>
    /// TODO:属性接口需要考虑Buff接口
    /// </summary>
    public interface IUnitAttribute
    {
        public string AttributeType { get; }

        public IUnitInfo OwnerUnitInfo { get; }

        public float FinalValue { get; }

        public int FinalIntegerValue { get; }
    }
}