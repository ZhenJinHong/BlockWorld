using System;
using System.IO;
using UnityEngine;

namespace CatFramework.SLMiao
{
    static partial class SaveAndLoad
    {
        #region 读取保存，通过Unity
        public static bool SaveByUniJsonNeedFileName(string fileFullName, object obj, params string[] paths)
        {
            if (TryCreateFileSavePath(fileFullName, out string filePath, paths))
            {
                File.WriteAllText(filePath, JsonUtility.ToJson(obj));
                return true;
            }
            return false;
        }
        #endregion
        #region 加载，通过Unity
        /// <summary>
        /// 此项是必须有解析类的，所以不会有空应用情况，会直接有默认值，但需要判断下类内部字段是否空的
        /// </summary>
        [Obsolete]
        public static bool LoadByUniJson<T>(out T obj, params string[] paths) where T : new()//避免数组的序列化
        {
            if (TryCheckFileLoadPath(out string filePath, paths))
            {
                string t = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(t))
                {
                    obj = JsonUtility.FromJson<T>(t);
                    return true;
                }
            }
            obj = default;
            return false;
        }
        /// <summary>
        /// 此项是必须有解析类的，会直接有默认值，但需要判断下类内部字段是否空的
        /// </summary>
        public static bool LoadByUniJson(out object obj, Type type, string filePath)
        {
            obj = null;
            if (File.Exists(filePath))
            {
                string t = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(t))
                {
                    obj = JsonUtility.FromJson(t, type);
                }
            }
            return obj != null;
        }
        /// <summary>
        /// 此项是必须有解析类的，会直接有默认值，但需要判断下类内部字段是否空的
        /// </summary>
        public static bool LoadByUniJson(out object obj, Type type, params string[] paths)
        {
            obj = null;
            if (TryCheckFileLoadPath(out string filePath, paths))
            {
                string t = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(t))
                {
                    obj = JsonUtility.FromJson(t, type);
                }
            }
            return obj != null;
        }
        #endregion
    }
}
