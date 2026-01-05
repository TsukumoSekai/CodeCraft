using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class TextureUtility
    {
        private const bool GenerateMipChain = true;

        public const string PNGSuffix = ".png";
        public const string DXTSuffix = ".dxt";

        private static ObjectPool<byte[]> s_BytesPool;

        static TextureUtility()
        {
            s_BytesPool = new ObjectPool<byte[]>(CreateBytes, null, ReleaseBytes, null, false, 3);
        }

        private static byte[] CreateBytes()
        {
            return new byte[4];
        }

        private static void ReleaseBytes(byte[] _bytes)
        {
            System.Array.Clear(_bytes, 0, 4);
        }

        /// <summary>
        /// 异步压缩成DXT
        /// </summary>
        /// <param name="_pngPath"></param>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        /// <returns></returns>
        public static async UniTask CompressToDXTAsync(string _pngPath, int _width, int _height)
        {
            Texture2D texture2D = new Texture2D(_width, _height, TextureFormat.ARGB32, GenerateMipChain);
            texture2D.LoadImage(await File.ReadAllBytesAsync(_pngPath));
            texture2D.Compress(false);

            byte[] imgWidth = System.BitConverter.GetBytes(texture2D.width);
            byte[] imgHeight = System.BitConverter.GetBytes(texture2D.height);
            byte[] imgFormat = System.BitConverter.GetBytes((int)texture2D.format);
            byte[] imgData = texture2D.GetRawTextureData();

            string dxtPath = _pngPath.Replace(PNGSuffix, DXTSuffix);

            await UniTask.SwitchToThreadPool();

            using (FileStream fileStream = new FileStream(dxtPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                await fileStream.WriteAsync(imgWidth, 0, 4);
                fileStream.Seek(4, SeekOrigin.Begin);
                await fileStream.WriteAsync(imgHeight, 0, 4);
                fileStream.Seek(8, SeekOrigin.Begin);
                await fileStream.WriteAsync(imgFormat, 0, 4);
                fileStream.Seek(12, SeekOrigin.Begin);
                await fileStream.WriteAsync(imgData, 0, imgData.Length);
            }
            //await UniTask.SwitchToMainThread();

            Object.Destroy(texture2D);

            Debug.Log($"压缩DXT:  {_pngPath}");
        }

        /// <summary>
        /// 批量异步压缩成DXT
        /// </summary>
        /// <param name="_rootFolderPath"></param>
        /// <param name="_subFolderList"></param>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        /// <returns></returns>
        public static async UniTask BatchCompressToDXTAsync(string _rootFolderPath, int _width, int _height)
        {
            string pngPath;
            int textureIndex;

            foreach (string subFolder in Directory.GetDirectories(_rootFolderPath))
            {
                textureIndex = 1;

                while (true)
                {
                    pngPath = Path.Combine(subFolder, $"{textureIndex++}{PNGSuffix}");

                    if (!File.Exists(pngPath))
                        break;

                    await CompressToDXTAsync(pngPath, _width, _height);
                }
            }
        }

        /// <summary>
        /// 同步压缩成DXT
        /// </summary>
        /// <param name="_pngPath"></param>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        public static void CompressToDXT(string _pngPath, int _width, int _height)
        {
            Texture2D texture2D = new Texture2D(_width, _height, TextureFormat.ARGB32, GenerateMipChain);
            texture2D.LoadImage(File.ReadAllBytes(_pngPath));
            texture2D.Compress(false);

            byte[] imgWidth = System.BitConverter.GetBytes(texture2D.width);
            byte[] imgHeight = System.BitConverter.GetBytes(texture2D.height);
            byte[] imgFormat = System.BitConverter.GetBytes((int)texture2D.format);
            byte[] imgData = texture2D.GetRawTextureData();

            string dxtPath = _pngPath.Replace(PNGSuffix, DXTSuffix);

            using (FileStream fileStream = new FileStream(dxtPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                fileStream.Write(imgWidth, 0, 4);
                fileStream.Seek(4, SeekOrigin.Begin);
                fileStream.Write(imgHeight, 0, 4);
                fileStream.Seek(8, SeekOrigin.Begin);
                fileStream.Write(imgFormat, 0, 4);
                fileStream.Seek(12, SeekOrigin.Begin);
                fileStream.Write(imgData, 0, imgData.Length);
            }

            Object.Destroy(texture2D);

            Debug.Log($"压缩DXT:  {_pngPath}");
        }

        /// <summary>
        /// 异步读取DXT
        /// </summary>
        /// <param name="_path"></param>
        /// <returns></returns>
        public static async UniTask<Texture2D> LoadDXTAsync(string _path)
        {
            if (!File.Exists(_path))
                return null;

            byte[] imgWidth = s_BytesPool.Get();
            byte[] imgHeight = s_BytesPool.Get();
            byte[] imgFormat = s_BytesPool.Get();

            byte[] imgData;

            await UniTask.SwitchToThreadPool();

            using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read))
            {
                int length = (int)fileStream.Length - 12;
                imgData = new byte[length];

                fileStream.Seek(0, SeekOrigin.Begin);
                await fileStream.ReadAsync(imgWidth, 0, 4);
                fileStream.Seek(4, SeekOrigin.Begin);
                await fileStream.ReadAsync(imgHeight, 0, 4);
                fileStream.Seek(8, SeekOrigin.Begin);
                await fileStream.ReadAsync(imgFormat, 0, 4);
                fileStream.Seek(12, SeekOrigin.Begin);
                await fileStream.ReadAsync(imgData, 0, length);
            }

            //await UniTask.SwitchToMainThread();

            var texture2D = new Texture2D(System.BitConverter.ToInt32(imgWidth, 0),
                                       System.BitConverter.ToInt32(imgHeight, 0),
                                       (TextureFormat)System.BitConverter.ToInt32(imgFormat, 0),
                                        true);

            texture2D.filterMode = FilterMode.Bilinear;
            texture2D.LoadRawTextureData(imgData);
            texture2D.Apply();
            texture2D.name = _path;

            return texture2D;
        }

        /// <summary>
        /// 同步读取DXT
        /// </summary>
        /// <param name="_path"></param>
        /// <returns></returns>
        public static Texture2D LoadDXT(string _path)
        {
            if (!File.Exists(_path))
                return null;

            byte[] imgWidth = s_BytesPool.Get();
            byte[] imgHeight = s_BytesPool.Get();
            byte[] imgFormat = s_BytesPool.Get();
            byte[] imgData;

            using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read))
            {
                int length = (int)fileStream.Length - 12;
                imgData = new byte[length];

                fileStream.Seek(0, SeekOrigin.Begin);
                fileStream.Read(imgWidth, 0, 4);
                fileStream.Seek(4, SeekOrigin.Begin);
                fileStream.Read(imgHeight, 0, 4);
                fileStream.Seek(8, SeekOrigin.Begin);
                fileStream.Read(imgFormat, 0, 4);
                fileStream.Seek(12, SeekOrigin.Begin);
                fileStream.Read(imgData, 0, length);
            }

            var texture2D = new Texture2D(System.BitConverter.ToInt32(imgWidth, 0),
                                       System.BitConverter.ToInt32(imgHeight, 0),
                                       (TextureFormat)System.BitConverter.ToInt32(imgFormat, 0),
                                        true);

            texture2D.filterMode = FilterMode.Bilinear;
            texture2D.LoadRawTextureData(imgData);
            texture2D.Apply();
            texture2D.name = _path;

            return texture2D;
        }
    }
}