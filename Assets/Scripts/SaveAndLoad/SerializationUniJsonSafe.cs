using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework.SLMiao
{
    public static class SerializationUniJsonSafe
    {
        #region 加载/保存
        /// <summary>
        /// 路径默认为所有存档父级路径C_Save/文件全名
        /// </summary>
        public static object LoadFormArchivePath(Type type, string fileFullName)
        {
            try
            {
                if (SaveAndLoad.LoadByUniJson(out object data, type, Paths.ArchivePath, fileFullName))
                {
                }
                return data;
            }
            catch (Exception ex)
            {
                ConsoleCat.LogWarning(ex);
            }
            return null;
        }
        /// <summary>
        /// 路径默认为所有存档父级路径C_Save/文件全名
        /// </summary>
        public static void SaveToArchivePath(object data, string fileFullName)
        {
            if (data != null)
            {
                try
                {
                    SaveAndLoad.SaveByUniJsonNeedFileName(fileFullName, data, Paths.ArchivePath);
                }
                catch (Exception ex)
                {
                    ConsoleCat.LogWarning(ex);
                }
            }
        }
        #endregion
        #region 配置的加载保存
        /// <summary>
        /// 读取单个Data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public static void SingleFile(out object data, Type type, string fileFullPath)
        {
            data = null;
            if (type.IsArray)
            {
                ConsoleCat.LogWarning("不可保存数组");
                return;
            }
            try
            {
                SaveAndLoad.LoadByUniJson(out data, type, fileFullPath);
            }
            catch (Exception e)
            {
                ConsoleCat.LogWarning(e);
            }
        }
        /// <summary>
        /// 读取单个Data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <param name="paths"></param>
        public static void SingleFile(out object data, Type type, params string[] paths)
        {
            data = null;
            if (type.IsArray)
            {
                ConsoleCat.LogWarning("不可保存数组");
                return;
            }
            try
            {
                SaveAndLoad.LoadByUniJson(out data, type, paths);
            }
            catch (Exception e)
            {
                ConsoleCat.LogWarning(e);
            }
        }
        /// <summary>
        /// 每个data（不同名称）保存在同文件夹直接的路径下
        /// </summary>
        public static void SingleFolderMultipleFiles(Type dataType, string searchConfigFileType, Action<object> complete, string parentfolderPath)
        {
            if (dataType == null || complete == null || dataType.IsArray)
            {
                ConsoleCat.LogWarning();
                return;
            }
            if (SaveAndLoad.TryGetFilePaths(searchConfigFileType, out string[] files, parentfolderPath))
            {
                int i = 0;
                while (i < files.Length)
                {
                    try
                    {
                        for (; i < files.Length; i++)
                        {
                            if (SaveAndLoad.LoadByUniJson(out object data, dataType, files[i]))
                            {
                                complete(data);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ConsoleCat.LogWarning(ex);
                    }
                    finally
                    {
                        i++;//跳过失败的
                    }
                }
            }
#if UNITY_EDITOR
            else
            {
                ConsoleCat.LogWarning();
            }
#endif
        }
        /// <summary>
        /// 当每个文件（名称相同）放在父级文件夹里每个子文件夹里时使用
        /// </summary>
        public static void MultipleChildrenFoldersSameFile(Type dataType, Action<object, string> dataAndItsFolderPath, string parentfolderPath, string fileFullName)
        {
            if (dataType == null || dataAndItsFolderPath == null || dataType.IsArray || string.IsNullOrWhiteSpace(fileFullName))
            {
                ConsoleCat.LogWarning();
                return;
            }
            if (SaveAndLoad.TryGetDirectoryChildPaths(out string[] folders, parentfolderPath))
            {
                int i = 0;
                while (i < folders.Length)
                {
                    try
                    {
                        for (; i < folders.Length; i++)
                        {
                            if (SaveAndLoad.LoadByUniJson(out object data, dataType, folders[i], fileFullName))
                            {
                                dataAndItsFolderPath(data, folders[i]);//回传数据后，再回传路径，路径可能是会放在这个数据里的，所以
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ConsoleCat.LogWarning(ex);
                    }
                    finally
                    {
                        i++;
                    }
                }
            }
#if UNITY_EDITOR
            else
            {
                ConsoleCat.LogWarning();
            }
#endif
        }
        /// <summary>
        /// 保存单个Data
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <param name="paths"></param>
        public static void SaveData(string fileFullName, object data, params string[] paths)
        {
            if (data.GetType().IsArray)
            {
                ConsoleCat.LogWarning("数组不可保存");
                return;
            }
            try
            {
                SaveAndLoad.SaveByUniJsonNeedFileName(fileFullName, data, paths);
            }
            catch (Exception e)
            {
                ConsoleCat.LogWarning(e);
            }
        }

        #endregion
    }
}
