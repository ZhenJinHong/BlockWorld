using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatFramework.Tools
{
    public class Texture2DArrayBuilder
    {
        List<Texture2D> texture2Ds;
        Texture2D defaultTex;
        int width, height, mipMapCount;
        bool IsValid(Texture2D texture2D)
        {
            return texture2D.width == width && texture2D.height == height && texture2D.mipmapCount == mipMapCount;
        }
        public Texture2DArrayBuilder()
        {
            texture2Ds = new List<Texture2D>();
        }
        public Texture2DArrayBuilder(int capacity)
        {
            texture2Ds = new List<Texture2D>(capacity);
        }
        public void Reset()
        {
            texture2Ds.Clear();
            defaultTex = null;
            width = 0;
            height = 0;
            mipMapCount = 0;
        }
        void SetDefaultTex(Texture2D defaultTex)
        {
            this.defaultTex = defaultTex;
            width = defaultTex.width;
            height = defaultTex.height;
            mipMapCount = defaultTex.mipmapCount;
        }
        public void Add(Texture2D texture, out int textureIndex)
        {
            textureIndex = 0;
            if (texture != null)
            {
                if (defaultTex == null)
                {
                    SetDefaultTex(texture);
                }
                if (IsValid(texture))
                {
                    textureIndex = texture2Ds.Count;
                    texture2Ds.Add(texture);
                }
            }
        }
        public Texture2DArray Create(bool normal)
        {
            if (defaultTex == null) return null;
            if (ConsoleCat.Enable)
            {
                ConsoleCat.Log($"当前的纹理Helper,默认贴图:{defaultTex};纹理mipMap数{mipMapCount}");
            }
            Texture2DArray texture2DArray = Create(defaultTex, texture2Ds, normal);
            return texture2DArray;
        }
        public static bool CheckTextureIsValid(IReadOnlyList<Texture2D> texture2Ds)
        {
            if (texture2Ds == null || texture2Ds.Count == 0) return false;
            Texture2D defaultTex = texture2Ds[0];
            int width = defaultTex.width, height = defaultTex.height, mipMapCount = defaultTex.mipmapCount;
            foreach (var tex in texture2Ds)
            {
                if (tex == null || tex.width != width || tex.height != height || tex.mipmapCount != mipMapCount)
                    return false;
            }
            return true;
        }
        public static Texture2DArray Create(Texture2D defaultTex, IReadOnlyList<Texture2D> texture2Ds, bool normal)
        {
            int mipMapCount = defaultTex.mipmapCount;
            Texture2DArray texture2DArray = new(defaultTex.width, defaultTex.height, texture2Ds.Count, defaultTex.format, mipMapCount, normal, true)
            { wrapMode = defaultTex.wrapMode, filterMode = defaultTex.filterMode };
            texture2DArray.Apply(false, true);

            for (int i = 0; i < texture2Ds.Count; i++)
            {
                Texture2D texture2D = texture2Ds[i];
                if (texture2D == null) texture2D = defaultTex;
                for (int m = 0; m < mipMapCount; m++)
                {
                    Graphics.CopyTexture(texture2D, 0, m, texture2DArray, i, m);
                }
            }
            return texture2DArray;
        }
    }
}
