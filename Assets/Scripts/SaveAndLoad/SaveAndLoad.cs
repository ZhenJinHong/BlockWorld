using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace CatFramework.SLMiao
{
    /// <summary>
    /// û��TryCatch
    /// </summary>
    public static partial class SaveAndLoad
    {
        #region Ŀ¼���ļ�����
        /// <summary>
        /// paths��Ӧ�����ļ���
        /// ��ϱ���·����������ļ�Ŀ���ļ����Ƿ���ڣ������򴴽�
        /// </summary>
        public static bool TryCreateFileSavePath(string fileFullName, out string filePath, params string[] paths)
        {
            filePath = null;
            if (string.IsNullOrWhiteSpace(fileFullName))
            {
                return false;
            }
            if (TryCombinePath(out string folderPath, paths))//����ʱ�Ȼ�ȡ�ļ���·��������Ƿ�����ļ���
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);//�������򴴽�
                }
                filePath = Path.Combine(folderPath, fileFullName);
            }
            return filePath != null;
        }
        /// <summary>
        /// ����ļ�����·�������жϸ�·���Ƿ����
        /// </summary>
        public static bool TryCheckFileLoadPath(out string filePath, params string[] paths)
        {
            return TryCombinePath(out filePath, paths) && File.Exists(filePath);
        }
        /// <summary>
        /// �����ھ���ȷ���ǿյ������ʹ�ã����򷵻�NULL
        /// </summary>
        public static string Combine(params string[] paths)
        {
            TryCombinePath(out string path, paths);
            return path;
        }
        /// <summary>
        /// ��һΪ�ջ�հ׷�������false
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
        /// ��ȡָ��·���ļ�������Ŀ¼�б�����·����
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
        /// �����ļ�·����Ŀ¼����
        /// </summary>
        /// <remarks>
        /// �����ʱ��Խ����Խǰ
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
            return y.LastWriteTime.CompareTo(x.LastWriteTime);//CompareTo()����������ڵ�С������-1
        }
        /// <summary>
        /// ��ȡ·����ĩ�����ƣ����·�����ļ�·�������ص��ļ�����������չ��
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
        /// ��ȡ·����ĩ�����ƣ����·�����ļ�·�������ص��ļ�����������չ��
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
        /// ��ȡ·����ĩ�����ƣ����·�����ļ�·�������ص��ļ�����������չ��
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
        /// ����Ŀ¼���ļ����б�����·����
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
        /// ����·���������ļ����б�����·����
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
        /// ɾ���ļ��� ?����Ӧ�þ���ʹ�ø��ǵķ�ʽ
        /// </summary>
        [Obsolete]
        public static bool TryDelfolder(string parentPath, string folderName)
        {
            //WARING!!!!�ر�ע��ɾ����ʱ����п���ɾ����һ�����ļ��У���Ϊ�浵��Ϊ�յ�ʱ����ɵ�·�������ϼ�·��
            if (TryCombinePath(out string path, parentPath,/*���п�*/ folderName) && Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, true);
                    return true;
                }
                catch (Exception e)
                {
                    ConsoleCat.LogWarning("ɾ���ļ���ʧ�ܣ�" + e.Message);
                }
            }
            return false;
        }
        /// <summary>
        /// ɾ���ļ�
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
                    ConsoleCat.LogWarning("ɾ���ļ�ʧ�ܣ�" + e.Message);
                }
            }
            return false;
        }
        /// <summary>
        /// ��ȡ�ļ����ļ��У��ϴ�д��ʱ���Ticks
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

        #region ��ȡ��д��
        #region ��ȡ
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
        #region д��
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