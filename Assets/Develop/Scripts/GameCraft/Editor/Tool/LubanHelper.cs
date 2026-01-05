using UnityEditor;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Editors
{
    public class LubanHelper : Editor
    {
        private static void RunBat(string batFile, string workingDir)
        {
            var path = FormatPath(workingDir + batFile);

            if (!System.IO.File.Exists(path))
            {
                Debug.LogError("bat文件不存在：" + path);
                return;
            }

            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.WorkingDirectory = workingDir;
                process.StartInfo.FileName = batFile;
                //proc.StartInfo.Arguments = args;
                //proc.StartInfo.CreateNoWindow = true;
                //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;//disable dos window
                process.Start();
                process.WaitForExit();
                process.Close();
            }
            catch (System.Exception ex)
            {
                Debug.LogFormat("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString());
            }
        }

        private static string FormatPath(string path)
        {
            path = path.Replace("/", "\\");
            if (Application.platform == RuntimePlatform.OSXEditor)
                path = path.Replace("\\", "/");
            return path;
        }

        [MenuItem("OfflineFantasy/Luban/打开数据库文件夹", priority = 0)]
        public static void OpenDatabaseFolder()
        {
            Application.OpenURL($"{Application.dataPath}/../Luban/MiniTemplate/Datas");
        }

        [MenuItem("OfflineFantasy/Luban/生成数据类和数据文件", priority = 1)]
        public static void GenerateDataClass()
        {
            RunBat("gen_new.bat", $"{Application.dataPath}/../Luban/MiniTemplate/");
        }
    }
}