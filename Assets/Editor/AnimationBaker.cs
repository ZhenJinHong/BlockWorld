using CatFramework;
using CatDOTS;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using System.Data;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace CatFramework.EditorTool
{
    public class AnimationBaker : EditorWindow
    {
        private void CreateGUI()
        {
            so_AnimationBaker ??= new SerializedObject(this);
            ObjectField objectField = new ObjectField(nameof(model));
            objectField.RegisterValueChangedCallback((v) =>
            {
                if (v.newValue is GameObject model)
                {
                    this.model = model;
                }
            });
            rootVisualElement.Add(objectField);

            PropertyField propertyField = new PropertyField(so_AnimationBaker.FindProperty(nameof(animationClipDatas)), nameof(AnimationClipData));
            propertyField.BindProperty(so_AnimationBaker);
            rootVisualElement.Add(propertyField);

            SliderInt textureWidthSlider = new SliderInt($"图片宽度：{TextureWidth}", 256, 4096);
            textureWidthSlider.SetValueWithoutNotify(TextureWidth);
            textureWidthSlider.RegisterValueChangedCallback((v) =>
            {
                TextureWidth = v.newValue;
                if (v.target is SliderInt sliderInt)
                {
                    sliderInt.SetValueWithoutNotify(TextureWidth);
                    sliderInt.label = $"图片宽度：{TextureWidth}";
                }
            });
            rootVisualElement.Add(textureWidthSlider);

            Toggle createTexutureAsset = new Toggle(nameof(CreateTextureAsset));
            createTexutureAsset.RegisterValueChangedCallback(v =>
            {
                CreateTextureAsset = v.newValue;
            });
            rootVisualElement.Add(createTexutureAsset);

            Toggle createMeshAsset = new Toggle(nameof(CreateMeshAsset));
            createMeshAsset.RegisterValueChangedCallback((v) =>
            {
                CreateMeshAsset = v.newValue;
            });
            rootVisualElement.Add(createMeshAsset);

            Toggle createVATAsset = new Toggle(nameof(CreateVATAsset));
            createVATAsset.RegisterValueChangedCallback((v) =>
            {
                CreateVATAsset = v.newValue;
            });
            rootVisualElement.Add(createVATAsset);

            Button bake = new Button(ClickBake)
            {
                text = "Bake"
            };
            rootVisualElement.Add(bake);
        }
        class AnimationBakerInfo
        {
            public SkinnedMeshRenderer skinnedMeshRenderer;
            public GameObject instance;
            public AnimationClipData[] animationClipDatas;

            public int textureWidth;

            public bool CreateTextureAsset;
            public bool CreateVATAsset;
            public bool CreateMeshAsset;
        }
        [System.Serializable]
        class AnimationClipData
        {
            public AnimationClip AnimationClip;
            public AnimationClipID AnimationClipType;
            [Range(12f, 30f)]
            public int FPS;
            public float EventPoint;
            public AnimationClipData()
            {
                FPS = 15;
            }
        }
        public GameObject model;
        [SerializeField] AnimationClipData[] animationClipDatas;
        int textureWidth = 2048;
        int TextureWidth
        {
            get { return textureWidth; }
            set
            {
                if (textureWidth != value)
                {
                    value = Mathf.Clamp(Mathf.NextPowerOfTwo(value), 256, 4096);
                    textureWidth = value;
                }
            }
        }

        bool CreateTextureAsset;
        bool CreateVATAsset;
        bool CreateMeshAsset;
        SerializedObject so_AnimationBaker;
        [MenuItem("CatFramework/VAT/AnimationBaker")]
        public static void ShowWindow()
        {
            GetWindow<AnimationBaker>();
        }
        //public void OnGUI()
        //{
        //    so_AnimationBaker ??= new SerializedObject(this);

        //    EditorGUILayout.PropertyField(so_AnimationBaker.FindProperty(nameof(AnimationBaker.model)));
        //    EditorGUILayout.PropertyField(so_AnimationBaker.FindProperty(nameof(AnimationBaker.animationClipDatas)));
        //    so_AnimationBaker.ApplyModifiedProperties();

        //    TextureWidth = Mathf.NextPowerOfTwo(EditorGUILayout.IntSlider(new GUIContent(nameof(TextureWidth)), TextureWidth, 256, 16384));

        //    CreateTextureAsset = EditorGUILayout.Toggle(new GUIContent(nameof(CreateTextureAsset)), CreateTextureAsset);
        //    CreateMeshAsset = EditorGUILayout.Toggle(new GUIContent(nameof(CreateMeshAsset)), CreateMeshAsset);
        //    CreateVATAsset = EditorGUILayout.Toggle(new GUIContent(nameof(CreateVATAsset)), CreateVATAsset);


        //    if (GUILayout.Button("Bake", GUILayout.Height(26)))
        //    {
        //        if (model == null ||
        //            animationClipDatas == null || animationClipDatas.Length == 0)
        //        {
        //            EditorUtility.DisplayDialog("Error", "缺失数据或实例乃资源", "OK");
        //            return;
        //        }
        //        GameObject instance = Instantiate(model);//如果自己实例化，需要找到一个新实例化的蒙皮渲染器
        //        instance.name = model.name;
        //        SkinnedMeshRenderer skinnedMeshRenderer = instance.GetComponentInChildren<SkinnedMeshRenderer>();
        //        if (skinnedMeshRenderer == null)
        //        {
        //            Debug.Log("对象上无蒙皮渲染器");
        //            return;
        //        }
        //        Bake(new AnimationBakerInfo()
        //        {
        //            instance = instance,
        //            skinnedMeshRenderer = skinnedMeshRenderer,
        //            animationClipDatas = animationClipDatas,

        //            textureWidth = TextureWidth,
        //            CreateTextureAsset = CreateTextureAsset,
        //            CreateVATAsset = CreateVATAsset,
        //            CreateMeshAsset = CreateMeshAsset
        //        });
        //        DestroyImmediate(instance);
        //    }
        //}
        void ClickBake()
        {
            if (model == null ||
                animationClipDatas == null || animationClipDatas.Length == 0)
            {
                Debug.LogWarning("缺失数据或实例乃资源");
                return;
            }
            GameObject instance = Instantiate(model);//如果自己实例化，需要找到一个新实例化的蒙皮渲染器
            instance.name = model.name;
            SkinnedMeshRenderer skinnedMeshRenderer = instance.GetComponentInChildren<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer == null)
            {
                Debug.Log("对象上无蒙皮渲染器");
                return;
            }
            Bake(new AnimationBakerInfo()
            {
                instance = instance,
                skinnedMeshRenderer = skinnedMeshRenderer,
                animationClipDatas = animationClipDatas,

                textureWidth = TextureWidth,
                CreateTextureAsset = CreateTextureAsset,
                CreateVATAsset = CreateVATAsset,
                CreateMeshAsset = CreateMeshAsset
            });
            DestroyImmediate(instance);
        }
        static void Bake(AnimationBakerInfo animationBakerInfo)
        {
            string folderPath = Path.Combine("Assets", "VATAsset", animationBakerInfo.instance.name);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            #region 创建图像
            int textureHeight = 0;
            int verticeCount = animationBakerInfo.skinnedMeshRenderer.sharedMesh.vertexCount;
            int pixelNumPerFrame = verticeCount / animationBakerInfo.textureWidth + (verticeCount % animationBakerInfo.textureWidth == 0 ? 0 : 1);
            for (int i = 0; i < animationBakerInfo.animationClipDatas.Length; i++)
            {
                AnimationClipData animationClipData = animationBakerInfo.animationClipDatas[i];
                if (animationClipData.AnimationClip == null)
                {
                    Debug.LogError("剪辑为空"); return;
                }
                if (animationClipData.FPS < 12) animationClipData.FPS = 15;
                //                                                帧数                                         //每帧行数
                textureHeight += Mathf.FloorToInt(animationClipData.FPS * animationClipData.AnimationClip.length) * pixelNumPerFrame;
            }
            textureHeight = Mathf.NextPowerOfTwo(textureHeight);
            Texture2D positionMap = new Texture2D(animationBakerInfo.textureWidth, textureHeight, TextureFormat.RGBAHalf, false, true);
            positionMap.filterMode = FilterMode.Point;
            #endregion
            #region 采样
            Vector3 Min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 Max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            Vector3 min = Min;
            Vector3 max = Max;
            Mesh sampleMesh = new Mesh();//样本网格
            List<Vector3> vertices = new List<Vector3>();//顶点列表
            List<Vector3> normals = new List<Vector3>();//法向顶点列表
            List<VATClip> vatClips = new List<VATClip>();//切片信息列表
            int y = 0;//像素Y行数值
            int totalFrames = 0;
            foreach (var animationClipData in animationBakerInfo.animationClipDatas)
            {
                var animationClip = animationClipData.AnimationClip;
                //总帧数；剪辑的长度表示秒//加上当前剪辑的帧数
                int frames = Mathf.FloorToInt(animationClipData.FPS * animationClip.length);
                totalFrames += frames;
                vatClips.Add(new VATClip()
                {
                    AnimationClipID = animationClipData.AnimationClipType,
                    Loop = animationClip.isLooping,
                    Length = animationClip.length,
                    //一秒除以每帧耗时 获得每秒实际帧数//animationClip.length是完整时长，实际的帧数并非烘焙信息里的FPS
                    FPS = 1 / (animationClip.length / frames),
                    EventPoint = animationClipData.EventPoint,
                    Frames = frames,
                    StartFrame = totalFrames - frames,
                    EndFrame = totalFrames - 1,
                });

                for (int currentFrame = 0; currentFrame < frames; currentFrame++)//当前帧一直增加，总小于总帧数
                {
                    // (animationClip.length / frames)每帧时长，i为第几帧，计算得出当前帧对应时间；
                    // animationClip.length是完整时长，实际的帧数并非烘焙信息里的FPS；因此上方VATClip帧数重算了
                    animationClip.SampleAnimation(animationBakerInfo.instance, (animationClip.length / frames) * currentFrame);
                    animationBakerInfo.skinnedMeshRenderer.BakeMesh(sampleMesh);
                    sampleMesh.GetVertices(vertices);
                    sampleMesh.GetNormals(normals);
                    int x = 0;//帧横轴
                    for (int j = 0; j < vertices.Count; j++)
                    {
                        min = Vector3.Min(min, vertices[j]);
                        max = Vector3.Max(max, vertices[j]);
                        positionMap.SetPixel(x, y, new Color(vertices[j].x, vertices[j].y, vertices[j].z, EncodeFloat3ToFloat1(normals[j])));

                        x++;
                        if (x >= animationBakerInfo.textureWidth)//超出图片大小，归零
                        {
                            x = 0;
                            y++;//纵轴加1
                        }
                    }
                    if (x != 0)//如果横轴不等于0，则有一行，暂未满，纵轴加1
                        y++;
                }
            }
            positionMap.Apply(false, true);
            #endregion
            if (animationBakerInfo.CreateTextureAsset)
            {
                string positionMapPath = Path.Combine(folderPath, "PositionMap.asset");
                AssetDatabase.CreateAsset(positionMap, positionMapPath);
            }//贴图是关键数据，如果没有贴图的数据，余下的无法正确计算
            if (animationBakerInfo.CreateMeshAsset && textureHeight != 0 && min != Min && max != Max)
            {
                string meshfilePath = Path.Combine(folderPath, "MsehLOD0.mesh");
                Mesh mesh = CopyMesh(animationBakerInfo.skinnedMeshRenderer.sharedMesh);
                mesh.bounds = new Bounds() { min = min, max = max };
                BakePositionUVs(mesh, animationBakerInfo.textureWidth, textureHeight);//烘焙UV需要图片的大小
                mesh.Optimize();//如果有调整网格或简化才需优化
                mesh.UploadMeshData(true);
                AssetDatabase.CreateAsset(mesh, meshfilePath);
            }
            if (animationBakerInfo.CreateVATAsset)
            {
                string vatAssetFilePath = Path.Combine(folderPath, animationBakerInfo.instance.name + "VATAsset.asset");
                VATAsset vatAsset = CreateInstance<VATAsset>();
                vatAsset.FrameHeightInMap = pixelNumPerFrame / (float)textureHeight;
                vatAsset.VATDatas = vatClips;
                AssetDatabase.CreateAsset(vatAsset, vatAssetFilePath);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        static float EncodeFloat3ToFloat1(Vector3 f3)
        {
            float f1;

            float z = Mathf.Sqrt(f3.z * 8 + 8);
            float y = (f3.y / z + 0.5f) * 31;
            float x = Mathf.Floor((f3.x / z + 0.5f) * 31) * 32;
            f1 = (x + y) / 1023;

            return f1;
        }
        static Vector2[] BakePositionUVs(Mesh mesh, int textureWidth, int textureHeight)
        {
#if UNITY_EDITOR
            Debug.Log($"图片大小{textureWidth}:{textureHeight};顶点数量:{mesh.vertexCount}");
#endif
            Vector2[] uv2 = new Vector2[mesh.vertexCount];

            float xOffset = 1.0f / textureWidth;
            float yOffset = 1.0f / textureHeight;//计算一像素宽高

            float x = xOffset / 2.0f;
            float y = yOffset / 2.0f;//半个像素宽高

            for (int v = 0; v < uv2.Length; v++)
            {
                uv2[v] = new Vector2(x, y);

                x += xOffset;
                if (x >= 1.0f)
                {
                    x = xOffset / 2.0f;
                    y += yOffset;//一行完毕，增加一个像素高度
                }
            }

            mesh.uv2 = uv2;

            return uv2;
        }
        static Mesh CopyMesh(Mesh original)
        {
            Mesh meshCopy = new Mesh();
            //meshCopy.Clear();
            meshCopy.name = original.name;
            meshCopy.indexFormat = original.indexFormat;
            meshCopy.vertices = original.vertices;
            meshCopy.triangles = original.triangles;
            meshCopy.normals = original.normals;
            meshCopy.tangents = original.tangents;
            meshCopy.colors = original.colors;
            meshCopy.bounds = original.bounds;
            meshCopy.uv = original.uv;
            meshCopy.uv2 = original.uv2;
            meshCopy.uv3 = original.uv3;
            meshCopy.uv4 = original.uv4;
            meshCopy.uv5 = original.uv5;
            meshCopy.uv6 = original.uv6;
            meshCopy.uv7 = original.uv7;
            meshCopy.uv8 = original.uv8;
            return meshCopy;
        }
    }
}