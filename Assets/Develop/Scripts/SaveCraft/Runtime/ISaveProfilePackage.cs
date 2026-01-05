using Cysharp.Threading.Tasks;

namespace OfflineFantasy.GameCraft.Saves
{
    public interface ISaveProfilePackage
    {
        public UniTask Save();
    }
}