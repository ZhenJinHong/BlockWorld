using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework.SLMiao
{
    public static class Serialization
    {
        public const BindingFlags DefaulfBindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        #region 对象转字符串
        //public static void ObjectToString(object obj, StringBuilder stringBuilder, BindingFlags bindingFlags = DefaulfBindFlags)
        //{
        //}
        public static void ObjectFieldToString(object obj, StringBuilder stringBuilder, BindingFlags bindingFlags = DefaulfBindFlags)
        {
            if (Assert.IsNull(obj)) return;
            Type type = obj.GetType();
            stringBuilder.AppendLine(type.FullName + " : ");

            FieldInfo[] fields = type.GetFields(bindingFlags);
            foreach (FieldInfo field in fields)
            {
                stringBuilder.AppendLine(field.Name + " : " + field.GetValue(obj));
            }
        }
        public static string ObjectFieldToString(object obj, BindingFlags bindingFlags = DefaulfBindFlags, int fieldLimitToUseBuilder = 16)
        {
            if (Assert.IsNull(obj)) return "";
            Type type = obj.GetType();

            FieldInfo[] fields = type.GetFields(bindingFlags);
            if (fields.Length > fieldLimitToUseBuilder)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(type.FullName + " : ");
                foreach (FieldInfo field in fields)
                {
                    stringBuilder.AppendLine(field.Name + " : " + field.GetValue(obj));
                }
                return stringBuilder.ToString();
            }
            else
            {
                string s = type.FullName + " : ";
                foreach (FieldInfo field in fields)
                {
                    s += "\n" + field.Name + " : " + field.GetValue(obj);
                }
                return s;
            }
        }
        public static void ObjectPropertyToString(object obj, StringBuilder stringBuilder, BindingFlags bindingFlags = DefaulfBindFlags)
        {
            if (Assert.IsNull(obj)) return;
            Type type = obj.GetType();
            stringBuilder.AppendLine(type.FullName + " : ");
            PropertyInfo[] propertyInfos = type.GetProperties(bindingFlags);
            foreach (var info in propertyInfos)
            {
                if (info.CanRead)
                {
                    stringBuilder.AppendLine(info.Name + " : " + info.GetValue(obj));
                }
            }
        }
        #endregion
        // 如果包括非公开字段会把默认实现的属性背后的字段也算上
        public static void CopyFieldWithObjectTypeDifferent(object source, object target, BindingFlags bindingFlags = DefaulfBindFlags)
        {
            if (Assert.IsNull(source) || Assert.IsNull(target) || source == target) return;
            FieldInfo[] fieldInfos = source.GetType().GetFields(bindingFlags);
            Type targetType = target.GetType();
            foreach (var info in fieldInfos)
            {
                FieldInfo targetInfo = targetType.GetField(info.Name, bindingFlags);
                if (targetInfo != null && targetInfo.FieldType == info.FieldType)
                    targetInfo.SetValue(target, info.GetValue(source));
            }
        }
        public static void CopyPropertyWithObjectTypeDifferent(object source, object target, BindingFlags bindingFlags = DefaulfBindFlags)
        {
            if (Assert.IsNull(source) || Assert.IsNull(target) || source == target) return;
            PropertyInfo[] propertyInfos = source.GetType().GetProperties(bindingFlags);
            Type targetType = target.GetType();
            foreach (var info in propertyInfos)
            {
                if (info.CanRead)
                {
                    PropertyInfo targetInfo = targetType.GetProperty(info.Name, bindingFlags);
                    if (targetInfo != null && targetInfo.CanWrite && targetInfo.PropertyType == info.PropertyType)
                        targetInfo.SetValue(target, info.GetValue(source));
                }
            }
        }
    }
}
