using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace CatFramework_TestDOTS.Assets.Scripts.CatFramework_TestDOTS.Test
{
    [Obsolete]
    public class NoiseTest : MonoBehaviour
    {
        [SerializeField, Range(10, 500)] int width;
        [SerializeField] float Scale;
        [SerializeField] Renderer Renderer;
        [SerializeField] NoiseType noiseType;
        [SerializeField] float XOffset;
        [SerializeField] float YOffset;
        private void OnValidate()
        {
            switch (noiseType)
            {
                case NoiseType.CNoise:
                    Show(noise.cnoise); break;
                case NoiseType.SNoise:
                    Show(noise.snoise); break;
            }
        }
        void Show(Func<float2, float> noise)
        {
            if (Scale <= 0) Scale = 0.001f;
            if (width < 1) width = 1;
            Color[] colors = new Color[width * width];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    colors[i * width + j] = Color.Lerp(Color.white, Color.black, noise(new float2(i, j) / Scale + new float2(XOffset, YOffset)));
                }
            }
            Texture2D texture2D = new Texture2D(width, width);
            texture2D.SetPixels(colors);
            texture2D.Apply();
            Renderer.sharedMaterial.mainTexture = texture2D;
        }
        enum NoiseType
        {
            CNoise,
            SNoise
        }
    }
}