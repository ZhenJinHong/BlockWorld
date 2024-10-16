using System;
using System.Collections.Generic;
using System.IO;

namespace CatFramework.SLMiao
{
    static partial class SaveAndLoad
    {
        public static bool SaveTextLines(string fileFullName, string[] texts, params string[] paths)
        {
            if (TryCreateFileSavePath(fileFullName, out string filePath, paths))
            {
                //这个是不会自动创建路径的
                File.WriteAllLines(filePath, texts);
                return true;
            }
            return false;
        }
        public static bool LoadTextLines(out string[] texts, params string[] paths)
        {
            if (TryCheckFileLoadPath(out string filePath, paths))
            {
                texts = File.ReadAllLines(filePath);
                return true;
            }
            texts = null;
            return false;
        }
        public static bool SaveAllText(string fileFullName, string text, params string[] paths)
        {
            if (TryCreateFileSavePath(fileFullName, out string filePath, paths))
            {
                File.WriteAllText(filePath, text);
                return true;
            }
            return false;
        }
        public static bool LoadAllText(out string text, params string[] paths)
        {
            text = null;
            if (TryCheckFileLoadPath(out string filePath, paths))
            {
                text = File.ReadAllText(filePath);
            }
            return text != null;
        }
    }
}
