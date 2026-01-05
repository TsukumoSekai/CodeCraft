using Cysharp.Threading.Tasks;

namespace OfflineFantasy.GameCraft.Saves
{
    public interface ISaveProfile
    {
        public UniTask Save();
    }
}