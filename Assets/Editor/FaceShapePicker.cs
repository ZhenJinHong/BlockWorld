using CatDOTS.VoxelWorld;
using CatFramework;
using CatFramework.UiTK;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.Entities.UniversalDelegates;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CatFramework.EditorTool
{
    #region //
    //[CustomPreview(typeof(VoxelShapeDefinition))]
    //public class VoxelShapeRectPreview : ObjectPreview
    //{
    //    VoxelShapeDefinition voxelShapeDefinition;
    //    Mesh rectMesh;
    //    MeshPreview meshPreview;
    //    public override void Cleanup()// 切换到其它的,就会执行
    //    {
    //        base.Cleanup();
    //        if (rectMesh != null)
    //            UnityEngine.Object.DestroyImmediate(rectMesh);
    //        if (meshPreview != null)
    //        {
    //            //Debug.Log("释放网格预览");
    //            meshPreview.Dispose();
    //        }
    //    }
    //    public override bool HasPreviewGUI()
    //    {
    //        //voxelShapeDefinition = target as VoxelShapeDefinition;
    //        //return voxelShapeDefinition.CheckRect;
    //        return true;
    //    }
    //    bool check;
    //    public override void OnPreviewSettings()
    //    {
    //        base.OnPreviewSettings();
    //        check |= GUILayout.Button("检查");
    //    }
    //    public override GUIContent GetPreviewTitle()
    //    {
    //        return base.GetPreviewTitle();
    //    }
    //    public override void OnPreviewGUI(Rect r, GUIStyle background)// 每次操作预览都执行
    //    {
    //        //base.OnPreviewGUI(r, background);
    //        if (check)
    //        {
    //            voxelShapeDefinition = target as VoxelShapeDefinition;
    //            if (rectMesh == null)
    //            {
    //                rectMesh = new Mesh();
    //                var shapes = voxelShapeDefinition.VoxelShapes;
    //                if (shapes != null && shapes.Count > 0)
    //                {
    //                    VoxelShapeElement voxelShapeElement = shapes[0];
    //                    ulong front = voxelShapeElement.frontRect;
    //                    List<Vector3> vertex = new List<Vector3>();
    //                    List<ushort> indexs = new List<ushort>();
    //                    for (int x = 0; x < 8; x++)
    //                    {
    //                        for (int y = 0; y < 8; y++)
    //                        {
    //                            int b = x * 8 + y;
    //                            if ((front & (1ul << b)) != 0)
    //                            {
    //                                int baseIndex = vertex.Count;
    //                                vertex.Add(new Vector3(0f, 0f, 0f));//原点
    //                                vertex.Add(new Vector3(0f, 1f * y, 0f));//左上
    //                                vertex.Add(new Vector3(1f * x, 1f * y, 0f));//右上
    //                                vertex.Add(new Vector3(1f * x, 0f, 0f));//左下
    //                                indexs.Add((ushort)(baseIndex + 0));
    //                                indexs.Add((ushort)(baseIndex + 1));
    //                                indexs.Add((ushort)(baseIndex + 2));
    //                                indexs.Add((ushort)(baseIndex + 2));
    //                                indexs.Add((ushort)(baseIndex + 3));
    //                                indexs.Add((ushort)(baseIndex + 0));
    //                            }
    //                        }
    //                    }
    //                    rectMesh.SetVertices(vertex);
    //                    rectMesh.SetIndices(indexs, MeshTopology.Triangles, 0);
    //                }
    //            }
    //            if (meshPreview == null)
    //            {
    //                meshPreview = new MeshPreview(rectMesh);
    //            }
    //            meshPreview.OnPreviewGUI(r, background);
    //        }
    //    }
    //}
    //public class VoxelShapeRectPreview : EditorWindow
    //{
    //    [MenuItem("CatFramework/VoxelWorld/VoxelShapeRectPreview")]
    //    public static void ShowWindow()
    //    {
    //        GetWindow<VoxelShapeRectPreview>();
    //    }
    //    Mesh mesh;
    //    Editor previewObj;
    //    MeshPreview meshPreview;
    //    VoxelShapeDefinition voxelShapeDefinition;
    //    private void CreateGUI()
    //    {
    //        ObjectField voxelShapeDefField = new ObjectField
    //        {
    //            label = nameof(VoxelShapeDefinition)
    //        };
    //        voxelShapeDefField.RegisterValueChangedCallback(DefChange);
    //        voxelShapeDefField.objectType = typeof(VoxelShapeDefinition);
    //        rootVisualElement.Add(voxelShapeDefField);
    //    }
    //    private void OnDestroy()
    //    {
    //        if (mesh != null)
    //            DestroyImmediate(mesh);
    //        if (previewObj != null)
    //            DestroyImmediate(previewObj);
    //        if (meshPreview != null)
    //            meshPreview.Dispose();
    //    }
    //    private void DefChange(ChangeEvent<UnityEngine.Object> evt)
    //    {
    //        var newV = evt.newValue as VoxelShapeDefinition;
    //        if (newV != voxelShapeDefinition)
    //        {
    //            voxelShapeDefinition = newV;
    //            if (mesh == null)
    //            {
    //                mesh = new Mesh();
    //            }
    //            if (meshPreview == null)
    //            {
    //                meshPreview = new MeshPreview(mesh);
    //            }
    //            if (voxelShapeDefinition != null)
    //            {
    //                //if (previewObj != null)
    //                //{
    //                //    DestroyImmediate(previewObj);
    //                //}
    //                //previewObj = Editor.CreateEditor(mesh);
    //                //if (previewObj.HasPreviewGUI())
    //                //{
    //                //    previewObj.OnPreviewGUI(GUILayoutUtility.GetRect(300, 300), EditorStyles.whiteLabel);
    //                //}
    //                meshPreview.OnPreviewGUI(GUILayoutUtility.GetRect(300, 300), EditorStyles.whiteLabel);
    //            }
    //        }
    //    }
    //}
    public class VoxelShapeEditorWindow : EditorWindow
    {
        [MenuItem("CatFramework/VoxelWorld/VoxelShapeEditor")]
        public static void ShowWindow()
        {
            GetWindow<VoxelShapeEditorWindow>();
        }
        [SerializeField] VoxelShapeDefinition voxelShapeDefinition;
        VoxelShapeDefinition VoxelShapeDefinition
        {
            get { return voxelShapeDefinition; }
            set
            {
                voxelShapeDefinition = value;
                normal.SetData(voxelShapeDefinition);
            }
        }
        FaceEditViewController faceEditViewController;
        MeshAndDataViewController normal;
        private void CreateGUI()
        {

            IStyle style = rootVisualElement.style;
            style.flexDirection = FlexDirection.Row;

            VisualElement matrixContainer = new VisualElement();
            rootVisualElement.Add(matrixContainer);

            ObjectField voxelShapeDefField = new ObjectField
            {
                label = nameof(VoxelShapeDefinition)
            };
            voxelShapeDefField.RegisterValueChangedCallback(DefChange);
            voxelShapeDefField.objectType = typeof(VoxelShapeDefinition);
            Button reRead = new Button
            {
                text = "刷新"
            };
            reRead.clicked += () =>
            {
                UnityEngine.Object o = voxelShapeDefField.value;
                voxelShapeDefField.value = null;
                voxelShapeDefField.value = o;
            };
            voxelShapeDefField.Add(reRead);
            matrixContainer.Add(voxelShapeDefField);

            faceEditViewController = new FaceEditViewController();
            matrixContainer.Add(faceEditViewController.Target);

            Button dirty = new Button();
            dirty.text = "保存更改";
            dirty.clicked += Save;
            matrixContainer.Add(dirty);

            VisualElement container = new VisualElement();
            normal = new MeshAndDataViewController(faceEditViewController);
            container.Add(normal.Target);
            rootVisualElement.Add(container);
        }
        void Save()
        {
            if (voxelShapeDefinition == null) return;
            EditorUtility.SetDirty(VoxelShapeDefinition); AssetDatabase.SaveAssetIfDirty(VoxelShapeDefinition);
        }
        void DefChange(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            VoxelShapeDefinition = changeEvent.newValue as VoxelShapeDefinition;
        }
        public static void UpdateText(Button button, ulong faceRect, int index)
        {
            button.text = GetText(faceRect, index);
        }
        public static string GetText(ulong faceRect, int index)
        {
            return ((faceRect & (1ul << index)) == (1ul << index) ? "■" : "□");
        }
        class FaceEditViewController : VisualElementController
        {
            ulong value;
            readonly Button[,] indexBtn;
            UnsignedLongField unsignedLongField;
            readonly List<FaceTypeUnion> faceTypeNames;
            class FaceTypeUnion
            {
                public string name;
                public FaceRect FaceRect;
            }
            public FaceEditViewController() : base(CreateContainer())
            {
                indexBtn = new Button[8, 8];
                VisualElement matrixContainer = new VisualElement();
                Add(matrixContainer);
                IStyle style = matrixContainer.style;
                style.alignContent = new StyleEnum<Align>(Align.Center);
                style.alignItems = new StyleEnum<Align>(Align.Center);
                style.width = 256;
                style.height = 256;
                for (int y = 7; y > -1; y--)
                {
                    VisualElement control = new VisualElement();
                    control.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                    matrixContainer.Add(control);
                    for (int x = 0; x < 8; x++)
                    {
                        Button button = new Button();
                        IStyle btnstyle = button.style;
                        btnstyle.marginTop = 2;
                        btnstyle.marginBottom = 2;
                        btnstyle.marginLeft = 2;
                        btnstyle.marginRight = 2;
                        btnstyle.borderLeftWidth = 0;
                        btnstyle.borderRightWidth = 0;
                        btnstyle.borderTopWidth = 0;
                        btnstyle.borderBottomWidth = 0;
                        btnstyle.width = 18;
                        btnstyle.height = 18;
                        button.clickable.clickedWithEventInfo += Click;
                        button.userData = y * 8 + x;
                        indexBtn[x, y] = button;
                        button.text = "□";
                        control.Add(button);
                    }
                }

                faceTypeNames = new List<FaceTypeUnion>();
                string[] faceTypes = Enum.GetNames(typeof(FaceRect));
                Array.Sort(faceTypes);
                for (int i = 0; i < faceTypes.Length; i++)
                {
                    string faceType = faceTypes[i];
                    if (faceType[0] != 'R')
                    {
                        faceTypeNames.Add(new FaceTypeUnion() { FaceRect = Enum.Parse<FaceRect>(faceType), name = faceType });
                    }
                }
                ListView faceTypeListView = new ListView(faceTypeNames, 18, MakeFaceTypeItem, BindFaceTypeItem);
                faceTypeListView.selectionChanged += SelectedFaceTypeItem;

                Add(faceTypeListView);
            }
            VisualElement MakeFaceTypeItem()
            {
                return new Label();
            }
            void BindFaceTypeItem(VisualElement visualElement, int index)
            {
                if (visualElement is Label label)
                {
                    label.text = faceTypeNames[index].name;
                }
            }
            void SelectedFaceTypeItem(IEnumerable<object> selected)
            {
                if (selected.First() is FaceTypeUnion faceTypeUnion)
                {
                    value = (ulong)faceTypeUnion.FaceRect;
                    if (unsignedLongField != null)
                        unsignedLongField.value = value;
                    UpdateMatrix();
                }

            }
            public void EditValue(UnsignedLongField unsignedLongField)
            {
                value = unsignedLongField.value;
                this.unsignedLongField = unsignedLongField;
                UpdateMatrix();
            }
            void UpdateMatrix()
            {
                for (int y = 7; y > -1; y--)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        Button button = indexBtn[x, y];
                        if (button.userData is int index)
                        {
                            UpdateText(button, value, index);
                        }
                        else
                        {
                            Debug.LogWarning($"{x}:{y}位置的按钮用户数据非int索引");
                        }
                    }
                }
            }
            void Click(EventBase eventBase)
            {
                if (eventBase.target is Button button && button.userData is int index)
                {
                    value ^= 1ul << index;
                    if (unsignedLongField != null)
                        unsignedLongField.value = value;
                    UpdateText(button, value, index);
                }
            }
        }
        class MeshAndDataViewController : VisualElementController
        {
            readonly ObjectField meshField;
            //Toggle multiShape;
            VoxelShapeDefinition definition;
            VoxelShapeElement current;
            readonly List<Button> indexBtn;
            readonly List<UnsignedLongField> unsignedLongFields;
            readonly FaceEditViewController faceEditVisualElement;
            public MeshAndDataViewController(FaceEditViewController faceEditVisualElement) : base(CreateContainer())
            {
                this.faceEditVisualElement = faceEditVisualElement;

                VisualElement indexBtnContaniner = new VisualElement();
                indexBtnContaniner.style.flexDirection = FlexDirection.Row;
                //multiShape = new Toggle();
                //multiShape.RegisterValueChangedCallback(ToggleMultiShape);
                //indexBtnContaniner.Add(multiShape);
                Add(indexBtnContaniner);
                indexBtn = new List<Button>();
                for (int i = 0; i < 8; i++)
                {
                    Button button = new Button();
                    button.text = i.ToString();
                    button.userData = i;
                    button.clickable.clickedWithEventInfo += SwitchShape;
                    indexBtnContaniner.Add(button);
                    indexBtn.Add(button);
                }

                meshField = new ObjectField();
                meshField.RegisterValueChangedCallback(SetMesh);
                meshField.objectType = typeof(Mesh);
                Add(meshField);
                FieldInfo[] fieldInfos = typeof(VoxelShapeElement).GetFields();
                unsignedLongFields = new List<UnsignedLongField>();
                for (int i = 0; i < fieldInfos.Length; i++)
                {
                    if (fieldInfos[i].FieldType == typeof(ulong))
                    {
                        UnsignedLongField unsignedLongField = CreateFaceRectField(fieldInfos[i].Name);
                        unsignedLongFields.Add(unsignedLongField);
                        Add(unsignedLongField);
                    }
                }
            }
            void SetMesh(ChangeEvent<UnityEngine.Object> changeEvent)
            {
                if (current != null)
                    current.Mesh = changeEvent.newValue as Mesh;
            }
            public void SetData(VoxelShapeDefinition voxelShapeDefinition)
            {
                this.definition = voxelShapeDefinition;
                //multiShape.SetValueWithoutNotify(definition != null && definition.VoxelShapes.Count == 7);
                SwitchShape(0);
            }
            void SwitchShape(int index)
            {
                if (definition != null && index > -1 && index < definition.VoxelShapes.Count)
                {
                    current = definition.VoxelShapes[index];
                    if (current != null)
                    {
                        meshField.SetValueWithoutNotify(current.Mesh);
                        FieldInfo[] fieldInfos = typeof(VoxelShapeElement).GetFields();
                        int i = 0;
                        foreach (var field in fieldInfos)
                        {
                            if (field.FieldType == typeof(ulong))
                            {
                                if (field.GetValue(current) is ulong faceRect)
                                    unsignedLongFields[i].SetValueWithoutNotify((ulong)faceRect);
                                i++;
                            }
                        }
                    }
                }
            }
            void SwitchShape(EventBase eventBase)
            {
                if (eventBase.target is VisualElement visualElement && visualElement.userData is int index)
                {
                    SwitchShape(index);
                }
            }
            UnsignedLongField CreateFaceRectField(string faceRectName)
            {
                UnsignedLongField unsignedLongField = new UnsignedLongField(faceRectName);
                unsignedLongField.RegisterValueChangedCallback(SetFaceRectValue);
                Button btn = new Button();
                btn.userData = unsignedLongField;
                btn.text = "编辑";
                btn.clickable.clickedWithEventInfo += EditorFace;
                unsignedLongField.Add(btn);
                return unsignedLongField;
            }
            void EditorFace(EventBase eventBase)
            {
                if (eventBase.target is Button { userData: UnsignedLongField unsignedLongField })
                {
                    faceEditVisualElement.EditValue(unsignedLongField);
                }
            }
            // 矩阵修改值的时候，会把值传给ulong字段，字段就会通知到这个方法里
            void SetFaceRectValue(ChangeEvent<ulong> changeEvent)
            {
                Type meshandData = typeof(VoxelShapeElement);
                FieldInfo fieldInfo = meshandData.GetField((changeEvent.target as UnsignedLongField).label);
                if (fieldInfo != null && fieldInfo.FieldType == typeof(ulong))
                    fieldInfo.SetValue(current, changeEvent.newValue);
            }
        }
    }
    #endregion
}