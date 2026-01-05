namespace OfflineFantasy.GameCraft.Utility.Interface
{
    public interface ISerializable<T>
    {
        public string Serialize(int _splitSymbolIndex = 0);

        //public T Deserialize(string _context, int _splitSymbolIndex = 0);

        public bool Deserialize(string _context, out T _object, int _splitSymbolIndex = 0);
    }
}