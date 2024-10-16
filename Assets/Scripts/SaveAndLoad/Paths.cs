using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatFramework.SLMiao
{
    public static class Paths
    {
        #region
        /// <summary>
        /// 将流文件下的路径和流文件夹路径组合，当inpath为空时返回空字符串
        /// </summary>
        public static string CombinePathWithStreamingAssetsPath(string inPath)
        {
            if (string.IsNullOrWhiteSpace(inPath))
            {
                ConsoleCat.LogWarning();
                return string.Empty;
            }
            return Path.Combine(Application.streamingAssetsPath, inPath);
        }
        public static string CombineWithModsPath(string inPath)
        {
            if (string.IsNullOrWhiteSpace(inPath))
            {
                ConsoleCat.LogWarning();
                return string.Empty;
            }
            return Path.Combine(Application.streamingAssetsPath, "mods", inPath);
        }
        #endregion
        #region 配置的路径 不要常量里放文件格式，如果方法里需要文件名，直接写入整个文件名，包含文件类型
        /// <summary>
        /// Application.streamingAssetsPath, "mods"//下分多个文件夹
        /// </summary>
        public readonly static string ModsFolderPath = Path.Combine(Application.streamingAssetsPath, "mods");
        /// <summary>
        /// 后缀，搜索模式
        /// </summary>
        public static readonly string searchType_Json = "*.json";
        //public const string Type_Json = ".json";
        /// <summary>
        /// 后缀，搜索模式
        /// </summary>
        public static readonly string searchType_cgst = "*.cgst";
        //public const string Type_cgst = ".cgst";
        #endregion
        #region 存档的路径
        public static string ArchiveParentFolder = "JingzaiCifang_Save";
        /// <summary>
        /// 直到Save文件夹C:/Users/43881/AppData/LocalLow/......\Cat_Save
        /// </summary>
        public static string ArchivePath =
#if UNITY_EDITOR
            Path.Combine(/*Application.persistentDataPath*/Application.dataPath, ArchiveParentFolder);
#else
            Path.Combine(Application.persistentDataPath, ArchiveParentFolder);
#endif

        #endregion
    }
}
