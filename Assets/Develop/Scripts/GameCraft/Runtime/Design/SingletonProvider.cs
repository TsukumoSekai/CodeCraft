using OfflineFantasy.GameCraft.Utility;

namespace OfflineFantasy.GameCraft.Design
{
    public class SingletonProvider<T> where T : class, new()
    {
        private SingletonProvider() { }

        private static T instance;
        private static readonly object synclock = new object();

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (synclock)
                    {
                        if (instance == null)
                        {
                            // 若T class具有私有构造函数,那么则无法使用SingletonProvider<T>来实例化new T();
                            instance = new T();

                            DebugCraft.Log($"实例化System单例, Class: { instance.GetType().ToString() }");
                        }
                    }
                }
                return instance;
            }
            set { instance = value; }
        }
    }
}