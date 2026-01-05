namespace OfflineFantasy.GameCraft.Saves
{
    public interface ISaveProfilePackageFactory<TSave> where TSave : ISaveProfilePackage
    {
        public TSave CreatePackage();
    }
}