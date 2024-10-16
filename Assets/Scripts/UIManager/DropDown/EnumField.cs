using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatFramework.UiMiao
{
    [RequireComponent(typeof(RectTransform))]
    public class EnumField : TInputField<Enum>, IPointerClickHandler
    {
        [SerializeField] TextMiao valuaText;
        EnumData enumData;
        Type enumType;
        public void SetEnumType(Type type)
        {
            enumType = type;
            enumData = EnumDatas.GetEnumData(type);
        }
        public bool CheckDataIsValid()
        {
            return enumData.names != null;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (CheckDataIsValid())
            {
                var d = UiManagerMiao.shareDropdownList;
                if (d != null)
                {
                    d.Show(Selected, enumData.names, transform as RectTransform);
                }
            }
        }
        void Selected(int index)
        {
            if (CheckDataIsValid() && Enum.TryParse(enumType, enumData.names[index], out object result) && result is Enum v)
            {
                SetValueWithoutNotify(v);
                onSubmit?.Invoke(this);
            }
        }
        public override void SetValueWithoutNotify(Enum value)
        {
            this.value = value;
            if (valuaText != null)
                valuaText.TranslationKey = value.ToString();
        }
    }
    public struct EnumData
    {
        public string[] names;
    }
    public static class EnumDatas
    {
        readonly static Dictionary<Type, EnumData> enumDatas = new Dictionary<Type, EnumData>();
        static EnumDatas()
        {

        }
        public static EnumData GetEnumData(Type type)
        {
            if (!type.IsEnum)
            {
                return default;
            }
            if (enumDatas.TryGetValue(type, out EnumData enumData))
                return enumData;
            enumData = default;
            enumData.names = Enum.GetNames(type);
            //enumData.values = (Enum[])Enum.GetValues(type);//返回是具体枚举类型而不是Enum
            return enumData;
        }
    }
}