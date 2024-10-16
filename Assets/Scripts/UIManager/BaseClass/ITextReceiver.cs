using UnityEngine;

namespace CatFramework.UiMiao
{
    public interface ITextReceiver
    {
        bool IsDestroy { get; }
        /// <summary>
        /// 仅接受一次按键输入
        /// </summary>
        bool OneKeyShoot { get; }
        string OriginalText { get; }
        RectTransform CoverageRect { get; }
        void OnValueChange(string text);
        void OnSubmit(string text);
        void OnEndEdit(string text);
    }
}
