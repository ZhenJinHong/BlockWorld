
using System;
namespace CatFramework.UiMiao
{
    /// <summary>
    /// 所有输入字段最多只有字段名可以翻译
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInputField<T> : IInputField
    {
        void SetValueWithoutNotify(T value);
        T GetValue();
        /// <summary>
        /// 设定值并默认发送事件，类内部不要调用，以免出现循环发送事件
        /// </summary>
        void SetValue(T value);
    }
    public interface IInputField
    {
        Type ValueType { get; }
        /// <summary>
        /// 输入字段的名，用以翻译
        /// </summary>
        string Label { get; set; }
    }
}
