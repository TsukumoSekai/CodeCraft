using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace OfflineFantasy.GameCraft.Saves
{
    public static class SaveController<TSave> where TSave : ISaveProfilePackage
    {
        private static int m_LastSlotIndex;

        public static ISaveConfig<TSave> SaveConfig { get; private set; }

        public static bool HasChanged { get; private set; }

        public static bool IsSaving { get; private set; }

        public static DateTime LastSaveTime { get; private set; }

        public static TimeSpan LastSaveTimeSpan => DateTime.Now - LastSaveTime;

        #region 私有方法

        /// <summary>a
        /// 注册自动存档计时器
        /// </summary>
        private static void RegisterAutoSave()
        {
            //TODO:由外部注册
            //GameTimeManager.Instance.RegisterTimer(string.Empty,
            //                                       GlobalDatabase.Instance.m_System_AutoSaveInterval,
            //                                       AutoSave);
        }

        /// <summary>
        /// 自动存档
        /// </summary>
        /// <param name="_param"></param>
        private static void AutoSave(string _param)
        {
            //TODO:需要有个存档条件  不满足条件则往后延迟数秒
            //if (!m_IsSaving && !UnitController.Instance.MainPlayerUnitEntity.IsInBattle)
            //    Save().Forget();

            //RegisterAutoSave();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_isSingletonSave"></param>
        /// <param name="_saveName"></param>
        /// <param name="_factory"></param>
        /// <param name="_serializerSettings"></param>
        public static void Initialize(ISaveConfig<TSave> _saveConfig)
        {
            //Debug.Log($"初始化存档控制器:  {typeof(TSave).Name}");
            SaveConfig = _saveConfig;
        }

        /// <summary>
        /// 获取存档
        /// </summary>
        /// <param name="_slotIndex"></param>
        /// <returns></returns>
        public static async UniTask<TSave> Get(int _slotIndex = 0)
        {
            string savePath = SaveConfig.GetSavePath(_slotIndex);

            if (!File.Exists(savePath))
            {
                Debug.LogError($"存档不存在:  {typeof(TSave).Name},  {savePath}");
                return default;
            }

            //return await m_SaveConfig.SaveHandler.Load(savePath, m_SaveConfig.SerializerSettings);

            string context = await SaveConfig.SaveHandler.Load(savePath);

            TSave package = JsonConvert.DeserializeObject<TSave>(context, SaveConfig.SerializerSettings);
            return package;
        }

        /// <summary>
        /// 删除存档
        /// </summary>
        /// <param name="_slotIndex"></param>
        public static void Delete(int _slotIndex = 0)
        {
            string savePath = SaveConfig.GetSavePath(_slotIndex);

            if (File.Exists(savePath))
                File.Delete(savePath);
            else
                Debug.LogError($"存档不存在:  {typeof(TSave).Name},  {savePath}");
        }

        /// <summary>
        /// 创建新档
        /// </summary>
        /// <param name="_slotIndex"></param>
        /// <returns></returns>
        public static async UniTask<TSave> Create(int _slotIndex = 0)
        {
            //TODO:先通知其他控制器初始化
            await SaveConfig.SaveHandler.BeforeCreate();

            TSave package = await Save(_slotIndex);

            await SaveConfig.SaveHandler.AfterCreate();

            //Debug.Log($"创建新档完成:  {typeof(TSave).Name}");

            return package;
        }

        /// <summary>
        /// 获取存档是否存在
        /// </summary>
        /// <param name="_slotIndex"></param>
        /// <returns></returns>
        public static bool IsExists(int _slotIndex = 0)
        {
            return File.Exists(SaveConfig.GetSavePath(_slotIndex));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="_slotIndex"></param>
        /// <returns></returns>
        public static async UniTask<TSave> Save(int _slotIndex = 0)
        {
            if (SaveConfig == null)
            {
                Debug.LogError($"存档控制器未进行初始化:  {typeof(TSave).Name}");
                return default;
            }

            if (!HasChanged && !SaveConfig.ForceSave)
                return default;

            if (IsSaving)
            {
                Debug.LogWarning($"上一个保存程序未完成, 不要频繁保存:  {typeof(TSave).Name}");
                return default;
            }

            IsSaving = true;

            TSave package = SaveConfig.PackageFactory.CreatePackage();
            await package.Save();

            string folderPath = SaveConfig.GetSaveFolderPath();

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string savePath = SaveConfig.GetSavePath(_slotIndex);
            string context = JsonConvert.SerializeObject(package, SaveConfig.SerializerSettings);

            await SaveConfig.SaveHandler.BeforeSave();

            await SaveConfig.SaveHandler.Save(savePath, context);

            //Debug.Log($"保存存档完成:  {typeof(TSave).Name},  {savePath}");

            IsSaving = false;
            HasChanged = false;

            LastSaveTime = DateTime.Now;

            await SaveConfig.SaveHandler.AfterSave();

            return package;
        }

        /// <summary>
        /// 在最后加载的存档位上保存
        /// </summary>
        /// <returns></returns>
        public static async UniTask<TSave> SaveInLastSlot()
        {
            return await Save(m_LastSlotIndex);
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="_slotIndex"></param>
        /// <returns></returns>
        public static async UniTask<TSave> Load(int _slotIndex = 0)
        {
            await SaveConfig.SaveHandler.BeforeLoad();

            TSave package = await Get(_slotIndex);

            //return await Load(package, _slotIndex);

            await SaveConfig.SaveHandler.AfterLoad(package);

            //Debug.Log($"加载存档完成:  {typeof(TSave).Name}");

            return package;
        }

        public static void SetDirty()
        {
            HasChanged = true;
        }

        ///// <summary>
        ///// 读取
        ///// </summary>
        ///// <param name="_package"></param>
        ///// <returns></returns>
        //public static async UniTask<TSave> Load(TSave _package, int _slotIndex = 0)
        //{
        //    m_LastSlotIndex = _slotIndex;

        //    if (_package == null)
        //        _package = await Load(_slotIndex);
        //    else
        //        await m_SaveConfig.SaveHandler.BeforeLoad();

        //    await m_SaveConfig.SaveHandler.AfterLoad(_package);

        //    Debug.Log($"读取存档完成:  {m_SaveConfig.GetSavePath(_slotIndex)}");

        //    return _package;
        //}

        #endregion
    }
}