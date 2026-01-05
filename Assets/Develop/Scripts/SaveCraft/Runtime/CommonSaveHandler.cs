//using Cysharp.Threading.Tasks;
//using KDream.DesktopPet.Saves;
//using Newtonsoft.Json;
//using System.IO;

//namespace OfflineFantasy.GameCraft.Saves
//{
//    public class CommonSaveHandler : ISaveHandler<GameSaveProfilePackage>
//    {
//        public UniTask BeforeCreate()
//        {
//            return default;
//        }

//        public UniTask AfterCreate()
//        {
//            return default;
//        }

//        public UniTask BeforeSave()
//        {
//            return default;
//        }

//        public async UniTask Save(string _path, string _content)
//        {
//            await File.WriteAllTextAsync(_path, _content, System.Text.Encoding.UTF8);

//            #region FileStream保存法

//            //using (FileStream fileStream = new FileStream(savingProfilePath, FileMode.OpenOrCreate, FileAccess.Write))
//            //{
//            //    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(context);
//            //    await fileStream.WriteAsync(buffer, 0, buffer.Length);
//            //}

//            #endregion
//        }

//        public UniTask AfterSave()
//        {
//            return default;
//        }

//        public UniTask BeforeLoad()
//        {
//            return default;
//        }

//        public async UniTask<string> Load(string _path)
//        {
//            return await File.ReadAllTextAsync(_path);

//            #region FileStream读取法

//            //StringBuilder stringBuilder = new StringBuilder();

//            //using (FileStream fileStream = new FileStream(savingProfilePath, FileMode.Open, FileAccess.Read))
//            //{
//            //    byte[] buffer = new byte[1024 * 1024];
//            //    int r;

//            //    while (true)
//            //    {
//            //        r = await fileStream.ReadAsync(buffer, 0, buffer.Length);
//            //        stringBuilder.Append(System.Text.Encoding.UTF8.GetString(buffer, 0, r));

//            //        if (r == 0)
//            //            break;
//            //    }
//            //}

//            //string context = stringBuilder.ToString();

//            #endregion
//        }

//        public UniTask AfterLoad(GameSaveProfilePackage _saveProfilePackage)
//        {
//            return default;
//        }

//        public UniTask<GameSaveProfilePackage> Load(string _path, JsonSerializerSettings _serializerSettings)
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}