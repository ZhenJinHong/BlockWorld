using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace CatFramework.SLMiao
{
    /// <summary>
    /// 没有TryCatch
    /// </summary>
    public static partial class SaveAndLoad
    {
        #region 目录，文件处理
        /// <summary>
        /// paths不应包括文件名
        /// 组合保存路径，并检测文件目标文件夹是否存在，否则则创建
        /// </summary>
        public static bool TryCreateFileSavePath(string fileFullName, out string filePath, params string[] paths)
        {
            filePath = null;
            if (string.IsNullOrWhiteSpace(fileFullName))
            {
                return false;
            }
            if (TryCombinePath(out string folderPath, paths))//保存时先获取文件夹路径，检查是否存在文件夹
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);//不存在则创建
                }
                filePath = Path.Combine(folderPath, fileFullName);
            }
            return filePath != null;
        }
        /// <summary>
        /// 组合文件加载路径，并判断该路径是否存在
        /// </summary>
        public static bool TryCheckFileLoadPath(out string filePath, params string[] paths)
        {
            return TryCombinePath(out filePath, paths) && File.Exists(filePath);
        }
        /// <summary>
        /// 仅限于绝对确定非空的情况下使用，否则返回NULL
        /// </summary>
        public static string Combine(params string[] paths)
        {
            TryCombinePath(out string path, paths);
            return path;
        }
        /// <summary>
        /// 任一为空或空白符，返回false
        /// </summary>
        public static bool TryCombinePath(out string path, params string[] paths)
        {
            path = null;
            if (paths.Length == 0)
            {
                return false;
            }
            if (paths.Length == 1)
            {
                path = paths[0];
                return !string.IsNullOrWhiteSpace(path);
            }
            for (int i = 0; i < paths.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(paths[i]))
                {
                    return false;
                }
            }
            path = Path.Combine(paths);
            return true;
        }
        /// <summary>
        /// 获取指定路径文件夹中子目录列表（包括路径）
        /// </summary>
        public static bool TryGetDirectoryChildPaths(out string[] folders, params string[] paths)
        {
            if (TryCombinePath(out string path, paths) && Directory.Exists(path))
            {
                folders = Directory.GetDirectories(path);
                return true;
            }
            folders = null;
            return false;
        }
        /// <summary>
        /// 返回文件路径子目录名称
        /// </summary>
        /// <remarks>
        /// 有序的时间越新排越前
        /// </remarks>
        public static bool TryGetDirectoryChildNames(List<string> container, params string[] paths)
        {
            if (TryCombinePath(out string path, paths) && Directory.Exists(path))
            {
                DirectoryInfo[] DirectoryInfos = new DirectoryInfo(path).GetDirectories();
                Array.Sort(DirectoryInfos, Compare);

                for (int i = 0; i < DirectoryInfos.Length; i++)
                {
                    container.Add(DirectoryInfos[i].Name);
                }
                return true;
            }
            return false;
        }
        static int Compare(DirectoryInfo x, DirectoryInfo y)
        {
            return y.LastWriteTime.CompareTo(x.LastWriteTime);//CompareTo()如果比括号内的小，返回-1
        }
        /// <summary>
        /// 获取路径最末的名称，如果路径是文件路径，返回的文件名不包括扩展名
        /// </summary>
        public static void GetPathNames(out string[] names, string[] paths)
        {
            names = new string[paths.Length];
            for (int i = 0; i < paths.Length; i++)
            {
                names[i] = Path.GetFileNameWithoutExtension(paths[i]);
            }
        }
        /// <summary>
        /// 获取路径最末的名称，如果路径是文件路径，返回的文件名不包括扩展名
        /// </summary>
        public static void GetPathNames(out string[] names, List<string> paths)
        {
            names = new string[paths.Count];
            for (int i = 0; i < paths.Count; i++)
            {
                names[i] = Path.GetFileNameWithoutExtension(paths[i]);
            }
        }
        /// <summary>
        /// 获取路径最末的名称，如果路径是文件路径，返回的文件名不包括扩展名
        /// </summary>
        public static void GetPathNames(out List<string> names, string[] path)
        {
            names = new List<string>();
            for (int i = 0; i < path.Length; i++)
            {
                names.Add(Path.GetFileNameWithoutExtension(path[i]));
            }
        }
        /// <summary>
        /// 返回目录内文件名列表（包含路径）
        /// </summary>
        public static bool TryGetFilePaths(string searchPattern, out string[] filePaths, params string[] paths)
        {
            if (!string.IsNullOrWhiteSpace(searchPattern) && TryCombinePath(out string path, paths) && Directory.Exists(path))
            {
                filePaths = Directory.GetFiles(path, searchPattern);
                return true;
            }
            filePaths = null;
            return false;
        }
        /// <summary>
        /// 返回路径中所有文件名列表（包含路径）
        /// </summary>
        public static bool TryGetFilePaths(out string[] filePaths, params string[] paths)
        {
            if (TryCombinePath(out string path, paths) && Directory.Exists(path))
            {
                filePaths = Directory.GetFiles(path);
                return true;
            }
            filePaths = null;
            return false;
        }
        /// <summary>
        /// 删除文件夹 ?或许应该尽量使用覆盖的方式
        /// </summary>
        [Obsolete]
        public static bool TryDelfolder(string parentPath, string folderName)
        {
            //WARING!!!!特别注意删除的时候会有可能删除上一级的文件夹，因为存档名为空的时候，组成的路径就是上级路径
            if (TryCombinePath(out string path, parentPath,/*已判空*/ folderName) && Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, true);
                    return true;
                }
                catch (Exception e)
                {
                    ConsoleCat.LogWarning("删除文件夹失败：" + e.Message);
                }
            }
            return false;
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        static bool TryDelfile(params string[] paths)
        {
            if (TryCombinePath(out string path, paths) && File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    return true;
                }
                catch (Exception e)
                {
                    ConsoleCat.LogWarning("删除文件失败：" + e.Message);
                }
            }
            return false;
        }
        /// <summary>
        /// 获取文件或文件夹，上次写入时间的Ticks
        /// </summary>
        public static void GetpathLastWriteTimesTicks(string[] paths, out long[] Tickss)
        {
            Tickss = new long[paths.Length];
            for (int i = 0; i < paths.Length; i++)
            {
                if (Directory.Exists(paths[i]))
                {
                    Tickss[i] = Directory.GetLastWriteTime(paths[i]).Ticks;
                }
                else
                {
                    Tickss[i] = 0;
                }
            }
        }
        #endregion

        #region 读取与写入
        #region 读取
        public static string[] ReadStr(int length, BinaryReader br)
        {
            string[] array = new string[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = br.ReadString();
            }
            return array;
        }
        public static char[] ReadChar(int length, BinaryReader br)
        {
            if (length < 1) return new char[0];
            return br.ReadChars(length);
        }
        public static int[] ReadInt(int length, BinaryReader br)
        {
            int[] array = new int[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = br.ReadInt32();
            }
            return array;
        }
        public static uint[] ReadUint(int length, BinaryReader br)
        {
            uint[] array = new uint[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = br.ReadUInt32();
            }
            return array;
        }
        public static double[] ReadDouble(int length, BinaryReader br)
        {
            double[] array = new double[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = br.ReadDouble();
            }
            return array;
        }
        public static float[] ReadSingle(int length, BinaryReader br)
        {
            float[] array = new float[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = br.ReadSingle();
            }
            return array;
        }
        public static long[] ReadLong(int length, BinaryReader br)
        {
            long[] array = new long[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = br.ReadInt64();
            }
            return array;
        }
        public static ulong[] ReadUlong(int length, BinaryReader br)
        {
            ulong[] array = new ulong[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = br.ReadUInt64();
            }
            return array;
        }
        public static bool[] ReadBoolean(int length, BinaryReader br)
        {
            bool[] array = new bool[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = br.ReadBoolean();
            }
            return array;
        }
        public static byte[] ReadBytes(int length, BinaryReader br)
        {
            if (length < 1) return new byte[0];
            return br.ReadBytes(length);
        }
        public static short[] ReadShort(int length, BinaryReader br)
        {
            short[] array = new short[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = br.ReadInt16();
            }
            return array;
        }
        public static ushort[] ReadUshort(int length, BinaryReader br)
        {
            ushort[] array = new ushort[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = br.ReadUInt16();
            }
            return array;
        }
        #endregion
        #region 写入
        public static void WriteStr(string[] strs, BinaryWriter bw)
        {
            for (int i = 0; i < strs.Length; i++)
            {
                bw.Write(strs[i]);
            }
        }
        public static void WriteChar(char[] chars, BinaryWriter bw)
        {
            if (chars.Length < 1) return;
            bw.Write(chars, 0, chars.Length);
        }
        public static void WriteInt(int[] ints, BinaryWriter bw)
        {
            for (int i = 0; i < ints.Length; i++)
            {
                bw.Write(ints[i]);
            }
        }
        public static void WriteUint(uint[] uints, BinaryWriter bw)
        {
            for (int i = 0; i < uints.Length; i++)
            {
                bw.Write(uints[i]);
            }
        }
        public static void WriteDouble(double[] doubles, BinaryWriter bw)
        {
            for (int i = 0; i < doubles.Length; i++)
            {
                bw.Write(doubles[i]);
            }
        }
        public static void WriteSingle(float[] floats, BinaryWriter bw)
        {
            for (int i = 0; i < floats.Length; i++)
            {
                bw.Write(floats[i]);
            }
        }
        public static void WriteLong(long[] longs, BinaryWriter bw)
        {
            for (int i = 0; i < longs.Length; i++)
            {
                bw.Write(longs[i]);
            }
        }
        public static void WriteUlong(ulong[] ulongs, BinaryWriter bw)
        {
            for (int i = 0; i < ulongs.Length; i++)
            {
                bw.Write(ulongs[i]);
            }
        }
        public static void WriteBoolean(bool[] bools, BinaryWriter bw)
        {
            for (int i = 0; i < bools.Length; i++)
            {
                bw.Write(bools[i]);
            }
        }
        public static void WriteBytes(byte[] bytes, BinaryWriter bw)
        {
            if (bytes.Length < 1) return;
            bw.Write(bytes, 0, bytes.Length);
        }
        public static void WriteShort(short[] shorts, BinaryWriter bw)
        {
            for (int i = 0; i < shorts.Length; i++)
            {
                bw.Write(shorts[i]);
            }
        }
        public static void WriteUshort(ushort[] ushorts, BinaryWriter bw)
        {
            for (int i = 0; i < ushorts.Length; i++)
            {
                bw.Write(ushorts[i]);
            }
        }
        #endregion
        #endregion
    }
}