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

            SliderInt textureWidthSlider = new SliderInt($"ͼƬ��ȣ�{TextureWidth}", 256, 4096);
            textureWidthSlider.SetValueWithoutNotify(TextureWidth);
            textureWidthSlider.RegisterValueChangedCallback((v) =>
            {
                TextureWidth = v.newValue;
                if (v.target is SliderInt sliderInt)
                {
                    sliderInt.SetValueWithoutNotify(TextureWidth);
                    sliderInt.label = $"ͼƬ��ȣ�{TextureWidth}";
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
        //            EditorUtility.DisplayDialog("Error", "ȱʧ���ݻ�ʵ������Դ", "OK");
        //            return;
        //        }
        //        GameObject instance = Instantiate(model);//����Լ�ʵ��������Ҫ�ҵ�һ����ʵ��������Ƥ��Ⱦ��
        //        instance.name = model.name;
        //        SkinnedMeshRenderer skinnedMeshRenderer = instance.GetComponentInChildren<SkinnedMeshRenderer>();
        //        if (skinnedMeshRenderer == null)
        //        {
        //            Debug.Log("����������Ƥ��Ⱦ��");
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
                Debug.LogWarning("ȱʧ���ݻ�ʵ������Դ");
                return;
            }
            GameObject instance = Instantiate(model);//����Լ�ʵ��������Ҫ�ҵ�һ����ʵ��������Ƥ��Ⱦ��
            instance.name = model.name;
            SkinnedMeshRenderer skinnedMeshRenderer = instance.GetComponentInChildren<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer == null)
            {
                Debug.Log("����������Ƥ��Ⱦ��");
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
            #region ����ͼ��
            int textureHeight = 0;
            int verticeCount = animationBakerInfo.skinnedMeshRenderer.sharedMesh.vertexCount;
            int pixelNumPerFrame = verticeCount / animationBakerInfo.textureWidth + (verticeCount % animationBakerInfo.textureWidth == 0 ? 0 : 1);
            for (int i = 0; i < animationBakerInfo.animationClipDatas.Length; i++)
            {
                AnimationClipData animationClipData = animationBakerInfo.animationClipDatas[i];
                if (animationClipData.AnimationClip == null)
                {
                    Debug.LogError("����Ϊ��"); return;
                }
                if (animationClipData.FPS < 12) animationClipData.FPS = 15;
                //                                                ֡��                                         //ÿ֡����
                textureHeight += Mathf.FloorToInt(animationClipData.FPS * animationClipData.AnimationClip.length) * pixelNumPerFrame;
            }
            textureHeight = Mathf.NextPowerOfTwo(textureHeight);
            Texture2D positionMap = new Texture2D(animationBakerInfo.textureWidth, textureHeight, TextureFormat.RGBAHalf, false, true);
            positionMap.filterMode = FilterMode.Point;
            #endregion
            #region ����
            Vector3 Min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 Max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            Vector3 min = Min;
            Vector3 max = Max;
            Mesh sampleMesh = new Mesh();//��������
            List<Vector3> vertices = new List<Vector3>();//�����б�
            List<Vector3> normals = new List<Vector3>();//���򶥵��б�
            List<VATClip> vatClips = new List<VATClip>();//��Ƭ��Ϣ�б�
            int y = 0;//����Y����ֵ
            int totalFrames = 0;
            foreach (var animationClipData in animationBakerInfo.animationClipDatas)
            {
                var animationClip = animationClipData.AnimationClip;
                //��֡���������ĳ��ȱ�ʾ��//���ϵ�ǰ������֡��
                int frames = Mathf.FloorToInt(animationClipData.FPS * animationClip.length);
                totalFrames += frames;
                vatClips.Add(new VATClip()
                {
                    AnimationClipID = animationClipData.AnimationClipType,
                    Loop = animationClip.isLooping,
                    Length = animationClip.length,
                    //һ�����ÿ֡��ʱ ���ÿ��ʵ��֡��//animationClip.length������ʱ����ʵ�ʵ�֡�����Ǻ決��Ϣ���FPS
                    FPS = 1 / (animationClip.length / frames),
                    EventPoint = animationClipData.EventPoint,
                    Frames = frames,
                    StartFrame = totalFrames - frames,
                    EndFrame = totalFrames - 1,
                });

                for (int currentFrame = 0; currentFrame < frames; currentFrame++)//��ǰ֡һֱ���ӣ���С����֡��
                {
                    // (animationClip.length / frames)ÿ֡ʱ����iΪ�ڼ�֡������ó���ǰ֡��Ӧʱ�䣻
                    // animationClip.length������ʱ����ʵ�ʵ�֡�����Ǻ決��Ϣ���FPS������Ϸ�VATClip֡��������
                    animationClip.SampleAnimation(animationBakerInfo.instance, (animationClip.length / frames) * currentFrame);
                    animationBakerInfo.skinnedMeshRenderer.BakeMesh(sampleMesh);
                    sampleMesh.GetVertices(vertices);
                    sampleMesh.GetNormals(normals);
                    int x = 0;//֡����
                    for (int j = 0; j < vertices.Count; j++)
                    {
                        min = Vector3.Min(min, vertices[j]);
                        max = Vector3.Max(max, vertices[j]);
                        positionMap.SetPixel(x, y, new Color(vertices[j].x, vertices[j].y, vertices[j].z, EncodeFloat3ToFloat1(normals[j])));

                        x++;
                        if (x >= animationBakerInfo.textureWidth)//����ͼƬ��С������
                        {
                            x = 0;
                            y++;//�����1
                        }
                    }
                    if (x != 0)//������᲻����0������һ�У���δ���������1
                        y++;
                }
            }
            positionMap.Apply(false, true);
            #endregion
            if (animationBakerInfo.CreateTextureAsset)
            {
                string positionMapPath = Path.Combine(folderPath, "PositionMap.asset");
                AssetDatabase.CreateAsset(positionMap, positionMapPath);
            }//��ͼ�ǹؼ����ݣ����û����ͼ�����ݣ����µ��޷���ȷ����
            if (animationBakerInfo.CreateMeshAsset && textureHeight != 0 && min != Min && max != Max)
            {
                string meshfilePath = Path.Combine(folderPath, "MsehLOD0.mesh");
                Mesh mesh = CopyMesh(animationBakerInfo.skinnedMeshRenderer.sharedMesh);
                mesh.bounds = new Bounds() { min = min, max = max };
                BakePositionUVs(mesh, animationBakerInfo.textureWidth, textureHeight);//�決UV��ҪͼƬ�Ĵ�С
                mesh.Optimize();//����е��������򻯲����Ż�
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
            Debug.Log($"ͼƬ��С{textureWidth}:{textureHeight};��������:{mesh.vertexCount}");
#endif
            Vector2[] uv2 = new Vector2[mesh.vertexCount];

            float xOffset = 1.0f / textureWidth;
            float yOffset = 1.0f / textureHeight;//����һ���ؿ��

            float x = xOffset / 2.0f;
            float y = yOffset / 2.0f;//������ؿ��

            for (int v = 0; v < uv2.Length; v++)
            {
                uv2[v] = new Vector2(x, y);

                x += xOffset;
                if (x >= 1.0f)
                {
                    x = xOffset / 2.0f;
                    y += yOffset;//һ����ϣ�����һ�����ظ߶�
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