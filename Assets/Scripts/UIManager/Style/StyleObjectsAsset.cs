using CatFramework.EventsMiao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CatFramework.UiMiao
{
    public abstract class StyleObject : ScriptableObject
    {
        public string Name => name;
        public abstract IDataEventLinked AsDataEventLinked();
        public virtual void Initialized(Style style) { }
    }
    public class Style
    {
        [NonSerialized]
        Dictionary<string, IDataEventLinked> styleDataMap;
        [NonSerialized]
        Font[] Fonts;
        public Font GetFont(int index) => Fonts[index];
        public Style(Font[] fonts)
        {
            styleDataMap = new Dictionary<string, IDataEventLinked>();
            Fonts = fonts;
        }
        public void RegisterStyleChange<T>(T styleObject, Action<T> action, bool applyStyle = true) where T : StyleObject
        {
            if (action != null && styleObject != null)
            {
                if (!TryGetStyleData<T>(styleObject.Name, out DataEventLinked<T> styleDataLinked))
                {
                    styleObject.Initialized(this);
                    styleDataLinked = styleObject.AsDataEventLinked() as DataEventLinked<T>;
                    if (!styleDataMap.TryAdd(styleObject.Name, styleDataLinked))
                    {
                        if (ConsoleCat.Enable)
                            ConsoleCat.LogWarning($"重名样式{styleObject.Name}");
                    }
                }
                styleDataLinked.Change += action;
                if (applyStyle)
                    action.Invoke(styleDataLinked.TValue);
            }
        }
        public void RegisterStyleChange<T>(string styleName, Action<T> action, bool applyStyle = true) where T : StyleObject
        {
            if (action != null && TryGetStyleData<T>(styleName, out DataEventLinked<T> styleDataLinked))
            {
                styleDataLinked.Change += action;
                if (applyStyle)
                    action.Invoke(styleDataLinked.TValue);
            }
        }
        public void UnregisterStyleChange<T>(T styleObject, Action<T> action) where T : StyleObject
        {
            UnregisterStyleChange<T>(styleObject.Name, action);
        }
        public void UnregisterStyleChange<T>(string styleName, Action<T> action) where T : StyleObject
        {
            if (action != null && TryGetStyleData<T>(styleName, out DataEventLinked<T> styleDataLinked))
            {
                styleDataLinked.Change -= action;
            }
        }
        public bool TryGetStyleData<T>(string styleName, out DataEventLinked<T> TStyleData) where T : StyleObject
        {
            TStyleData = null;
            if ((!string.IsNullOrEmpty(styleName)) && styleDataMap.TryGetValue(styleName, out IDataEventLinked styleDataLinked))
            {
                TStyleData = styleDataLinked as DataEventLinked<T>;
            }
            return TStyleData != null;
        }

        public Font GetFont(string name)
        {
            for (int i = 0; i < Fonts.Length; i++)
            {
                if (Fonts[i].name.Equals(name))
                {
                    return Fonts[i];
                }
            }
            return Fonts[0];
        }
        public int GetFontIndex(string name)
        {
            for (int i = 0; i < Fonts.Length; i++)
            {
                if (Fonts[i].name.Equals(name))
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
