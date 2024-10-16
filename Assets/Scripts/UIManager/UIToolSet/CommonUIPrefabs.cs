using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    public class CommonUIPrefabs : MonoBehaviour
    {
        [SerializeField] DropdownMiao dropdownMiao;
        [SerializeField] EnumField enumField;
        [SerializeField] StringInputField stringInputField;
        [SerializeField] IntInputField intInputField;
        [SerializeField] FloatInputField floatInputField;
        [SerializeField] BoolInputField boolInputField;
        [SerializeField] SelectColor selectColor;
        [SerializeField] UIntInputField uIntInputField;
        [SerializeField] ButtonField buttonField;
        //[SerializeField] UiItem[] fields;
        //Dictionary<Type, UiItem> fieldMap;
        private void Awake()
        {
            if (UiManagerMiao.commonUIPrefabs == null)
                UiManagerMiao.commonUIPrefabs = this;
        }
#if UNITY_EDITOR
        private void Start()
        {

        }
#endif
        private void OnDestroy()
        {
            if (UiManagerMiao.commonUIPrefabs == this)
                UiManagerMiao.commonUIPrefabs = null;
        }
        public DropdownMiao InstantiateDropdown(RectTransform parent) => Instantiate(dropdownMiao, parent);
        public EnumField InstantiateEnumField(RectTransform parent) => Instantiate(enumField, parent);
        public StringInputField InstantiateStringField(RectTransform parent) => Instantiate(stringInputField, parent);
        public IntInputField InstantiateIntField(RectTransform parent) => Instantiate(intInputField, parent);
        public FloatInputField InstantiateFloatField(RectTransform parent) => Instantiate(floatInputField, parent);
        public BoolInputField InstantiateBoolField(RectTransform parent) => Instantiate(boolInputField, parent);
        public SelectColor InstantiateColorField(RectTransform parent) => Instantiate(selectColor, parent);
        public UIntInputField InstantiateUintField(RectTransform parent) => Instantiate(uIntInputField, parent);
        public ButtonField InstantiateButtonField(RectTransform parent) => Instantiate(buttonField, parent);
        public void InstantiateField(RectTransform parent, object data)
        {
            Type dataType = data.GetType();
            PropertyInfo[] propertyInfos = dataType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var item in propertyInfos)
            {
                if (item.CanWrite && item.CanRead)
                {
                    if (item.IsDefined(typeof(DropdownAttribute)))
                    {
                        if (item.GetCustomAttribute(typeof(DropdownAttribute)) is DropdownAttribute dropdownAttribute
                            && dropdownAttribute.methodName != null)
                        {
                            var methodInfo = dataType.GetMethod(dropdownAttribute.methodName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                            if (methodInfo != null
                                && methodInfo.GetParameters().Length == 0
                                && methodInfo.Invoke(data, null) is string[] options)
                            {
                                DropdownMiao field = Instantiate(dropdownMiao, parent);
                                field.AddOptions(options);
                                SetData(item, data, field);
                            }
                        }
                    }
                    else if (item.PropertyType.IsEnum)
                    {
                        EnumField field = Instantiate(enumField, parent);

                        field.SetEnumType(item.PropertyType);
                        SetData(item, data, field);
                    }
                    else if (item.PropertyType.IsValueType)
                    {
                        if (item.PropertyType == typeof(int))
                        {
                            NewField(item, data, intInputField, parent);
                        }
                        else if (item.PropertyType == typeof(float))
                        {
                            NewField(item, data, floatInputField, parent);
                        }
                        else if (item.PropertyType == typeof(bool))
                        {
                            NewField(item, data, boolInputField, parent);
                        }
                        else if (item.PropertyType == typeof(uint))
                        {
                            NewField(item, data, uIntInputField, parent);
                        }
                        else if (item.PropertyType == typeof(Color))
                        {
                            NewField(item, data, selectColor, parent);
                        }
                    }
                }
            }
        }
        public static void NewField<T>(PropertyInfo propertyInfo, object data, TInputField<T> prefab, RectTransform parent)
        {
            var field = Instantiate(prefab, parent);
            SetData(propertyInfo, data, field);
        }
        static void SetData<T>(PropertyInfo propertyInfo, object data, TInputField<T> field)
        {
            field.Label = propertyInfo.Name;
            field.OnSubmit += FieldCallBack<T>;
            if (field is TextInputField<T> tfield) tfield.OnEndEdit += FieldCallBack;
            if (propertyInfo.GetValue(data) is T value)
                field.SetValueWithoutNotify(value);
            field.useData = data;
        }
        public static void FieldCallBack<T>(TInputField<T> field)
        {
            //Debug.Log("字段回调");
            if (field.useData is object data)
            {
                //Debug.Log("字段获取");
                var propertyInfo = data.GetType().GetProperty(field.Label);
                if (propertyInfo != null)
                {
                    //Debug.Log($"字段类型{propertyInfo.PropertyType}");
                    if (field.GetValue() != null && propertyInfo.PropertyType == field.GetValue().GetType())
                    {
                        //Debug.Log("字段设置");
                        propertyInfo.SetValue(data, field.GetValue());
                        if (propertyInfo.GetValue(data) is T value)
                            field.SetValueWithoutNotify(value);
                    }
                }
                //else
                //{
                //    Debug.Log("字段为空");
                //}
            }
        }
    }
}