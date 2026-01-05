using Newtonsoft.Json;

namespace OfflineFantasy.GameCraft.Saves
{
    public interface ISaveConfig<TSave> where TSave : ISaveProfilePackage
    {
        public ISaveProfilePackageFactory<TSave> PackageFactory { get; }

        public JsonSerializerSettings SerializerSettings { get; }

        public ISaveHandler<TSave> SaveHandler { get; }

        public string GetSaveFolderPath();

        public string GetSavePath(int _slotIndex = 0);

        public bool ForceSave { get; }
    }
}