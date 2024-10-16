using CatFramework.UiMiao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VoxelWorld.UGUICTR;

namespace CatFramework.EditorTool
{
    internal abstract class UGUIMiaoBaseEditor<UiItem, Style> : Editor where UiItem : UiItem<Style> where Style : StyleObject
    {
        ObjectField objectField;
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement container = new VisualElement();
            objectField = new ObjectField("样式");
            objectField.objectType = typeof(Style);
            objectField.bindingPath = serializedObject.FindProperty("styleObj").propertyPath;
            objectField.RegisterValueChangedCallback(StyleChange);
            container.Add(objectField);
            Button button = new Button(Reflush);
            button.text = "刷新";
            objectField.Add(button);
            InspectorElement.FillDefaultInspector(container, serializedObject, this);

            return container;
        }
        void Reflush()
        {
            if (objectField.value is Style styleObject && serializedObject.targetObject is UiItem miao)
            {
                //miao.StyleName = styleObject.Name;
                //miao.SetStyleInEditor(styleObject);
                miao.ApplyStyle(styleObject);
                //EditorUtility.SetDirty(miao);
            }
        }
        void StyleChange(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            Reflush();
        }
    }
    //[CustomEditor(typeof(TextStyleObject))]
    //internal class TextStyleObjectEditor : Editor
    //{
    //    public override VisualElement CreateInspectorGUI()
    //    {
    //        VisualElement container = new VisualElement();
    //        ObjectField objectField = new ObjectField("font");
    //        objectField.objectType = typeof(Font);
    //        objectField.RegisterValueChangedCallback(FontChange);
    //        container.Add(objectField);
    //        InspectorElement.FillDefaultInspector(container, serializedObject, this);
    //        return container;
    //    }
    //    void FontChange(ChangeEvent<UnityEngine.Object> changeEvent)
    //    {
    //        if (changeEvent.newValue is Font font&&serializedObject.targetObject is TextStyleObject textStyleObject)
    //        {
    //            textStyleObject.StyleData.FontName = font.name;
    //            textStyleObject.StyleData.FontName = font.name;
    //        }
    //    }
    //}

    [CustomEditor(typeof(TextMiao))]
    internal class TextMiaoEditor : UGUIMiaoBaseEditor<TextMiao, TextStyleObject>
    {

    }
    [CustomEditor(typeof(ButtonMiao))]
    internal class ButtonMiaoEditor : UGUIMiaoBaseEditor<ButtonMiao, MaskableGraphicStyleObject>
    {

    }
    [CustomEditor(typeof(MaskableGraphicMiao))]
    internal class MaskableGraphicStyleEditor : UGUIMiaoBaseEditor<MaskableGraphicMiao, MaskableGraphicStyleObject>
    {

    }
    [CustomEditor(typeof(Ulattice))]
    internal class UlatticeStyleEditor : UGUIMiaoBaseEditor<Ulattice, MaskableGraphicStyleObject>
    {

    }
    [CustomEditor(typeof(MainMenuItem))]
    internal class MainMenuItemEditor : UGUIMiaoBaseEditor<MainMenuItem, MaskableGraphicStyleObject>
    {

    }
}
