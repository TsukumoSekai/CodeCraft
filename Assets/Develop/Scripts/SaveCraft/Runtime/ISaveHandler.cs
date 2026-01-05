using Cysharp.Threading.Tasks;

namespace OfflineFantasy.GameCraft.Saves
{
    public interface ISaveHandler<TSave> where TSave : ISaveProfilePackage
    {
        public UniTask BeforeCreate();

        public UniTask AfterCreate();

        public UniTask BeforeSave();

        public UniTask Save(string _path, string _content);

        public UniTask AfterSave();

        public UniTask BeforeLoad();

        public UniTask<string> Load(string _path);

        public UniTask AfterLoad(TSave _saveProfilePackage);
    }
}