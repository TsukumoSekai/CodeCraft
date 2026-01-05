using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Editors
{
    public static class LayerConstantGenerator
    {
        private const string m_ClassName = "LayerConstant";

        private static string FileName => new string($"{m_ClassName}.cs");

        private const int m_ = 0;

        [MenuItem("OfflineFantasy/工具/生成Layer常量")]
        public static void Generate()
        {
            #region 代码

            StringBuilder fields = new();
            string fieldName;

            for (int i = 0; i < 32; i++)
            {
                fieldName = LayerMask.LayerToName(i);

                if (string.IsNullOrEmpty(fieldName))
                    continue;

                fields.AppendLine($"\t\tpublic const int m_{fieldName.Replace(" ", string.Empty)} = {i};");
            }

            string codeContents = $@"namespace OfflineFantasy.GameCraft
{{
    public static class {m_ClassName}
    {{
{fields}
    }}
}}
";

            #endregion

            #region 路径

            string scriptPath = string.Empty;

            var guidArray = AssetDatabase.FindAssets(m_ClassName);

            foreach (var guid in guidArray)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);

                if (path.EndsWith(FileName))
                    scriptPath = path;
            }

            if (string.IsNullOrEmpty(scriptPath))
                scriptPath = $"{Application.dataPath}\\{FileName}";

            #endregion

            File.WriteAllText(scriptPath, codeContents, System.Text.Encoding.UTF8);

            AssetDatabase.Refresh();

            UnityEngine.Debug.Log($"{m_ClassName}生成成功,  {scriptPath}");

            //foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    foreach (var type in assembly.GetTypes())
            //    {
            //        if(type.Name == m_ClassName)
            //            scriptPath = Path.GetDirectoryName()
            //    }
            //}
        }
    }
}