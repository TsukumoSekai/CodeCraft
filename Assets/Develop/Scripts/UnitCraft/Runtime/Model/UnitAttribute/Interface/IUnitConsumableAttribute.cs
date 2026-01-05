namespace OfflineFantasy.GameCraft.Unit.Model
{
    public interface IUnitConsumableAttribute : IUnitAttribute
    {
        public float CurrentValue { set; get; }

        public float CurrentValueUpperLimit { get; }
        public float CurrentValueLowerLimit { get; }
        public float CurrentPercentage { get; }

        public int CurrentIntegerValue { get; }
    }
}