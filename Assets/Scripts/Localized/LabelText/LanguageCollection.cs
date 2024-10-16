using CatFramework.SLMiao;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatFramework.Localized
{
    public class Language : ILanguage//如果改为结构体要重写equals
    {
        public string Original { get; private set; }
        public string Translation { get; private set; }
        public Language(string original, string translation)
        {
            Original = original;
            SetContent(translation);
        }
        public void SetContent(string translation)
        {
            Original ??= "";
            Translation = translation;
        }
    }
    public class LanguageCollection : ILanguageCollection
    {
        [NonSerialized]
        readonly Dictionary<string, Language> languageDic;
        [NonSerialized]
        readonly LocalizedDataConfigs localizedDataHandle;
        readonly Language emptyLanguage = new Language(string.Empty, string.Empty);
        const string PrefsKey = "C语言";
        public LanguageCollection()
        {
            languageDic = new Dictionary<string, Language>();
            localizedDataHandle = new LocalizedDataConfigs();

            PlayerPrefs.GetString(PrefsKey, m_CurrentLang);//没有获取到则null
            m_CurrentLang ??= Application.systemLanguage switch
            {
                SystemLanguage.Chinese => DefaultLang,
                SystemLanguage.ChineseSimplified => DefaultLang,
                SystemLanguage.English => "English",
                _ => DefaultLang,
            };
            Update();
        }
        public void Dispose()
        {
            languageDic.Clear();
            localizedDataHandle.Clear();
        }
        /// <summary>
        /// 保存选定了何种语言
        /// </summary>
        public void Save()
        {
            PlayerPrefs.SetString(PrefsKey, m_CurrentLang);
        }
        bool isWorking;
        /// <summary>
        /// 当前语言
        /// </summary>
        string m_CurrentLang;
        string DefaultLang => "中文简体";
        public int KeyMaxLength = 16;
        public string CurrentLang
        {
            get
            {
                if (string.IsNullOrEmpty(m_CurrentLang))
                {
                    m_CurrentLang = DefaultLang;
                }
                return m_CurrentLang;
            }
            set
            {
                m_CurrentLang = value;
            }
        }
        public void GetAllLanguageNames(List<string> names)
        {
            names.Clear();
            foreach (LocalizedDataConfig config in localizedDataHandle)
            {
                names.Add(config.LanguageName);
            }
        }
        /// <summary>
        /// 不抛出语言切换事件的！！
        /// </summary>
        public bool SwitchLanguageWithoutNotify(string languageType)
        {
            if (isWorking)
            {
#if UNITY_EDITOR
                ConsoleCat.BusyWarning();
#endif
                return false;
            }
            if (string.IsNullOrWhiteSpace(languageType) || CurrentLang == languageType) return false;
            CurrentLang = languageType;
            isWorking = true;
            Update();
            isWorking = false;
            return true;
        }
        /// <summary>
        /// 传来需要翻译的，获取译文
        /// </summary>
        public string Translate(string key)
        {
            return GetLanguage(key).Translation;
        }
        public bool KeyIsValid(string key)
        {
            if (key.Length > KeyMaxLength)
            {
                if (ConsoleCat.Enable)
                    ConsoleCat.LogWarning($"原文过长 : {key.Length}");
                return false;
            }
            return true;
        }
        public ILanguage GetLanguage(string origianl)
        {
            if (string.IsNullOrEmpty(origianl)) return emptyLanguage;
            if (!KeyIsValid(origianl)) return /*new Language(origianl, origianl)*/
                    emptyLanguage;
            if (!languageDic.TryGetValue(origianl, out Language language))
            {
                language = new Language(origianl, origianl);
                languageDic[origianl] = language;
            }
            return language;
        }
        public string GetHintText(string key)
        {
            if (isWorking)
            {
#if UNITY_EDITOR
                ConsoleCat.BusyWarning();
#endif
                return key;
            }
            if (string.IsNullOrEmpty(key))
            {
                ConsoleCat.LogWarning("翻译键为空");
                return "null";
            }
            return key;
        }
        void Update()
        {
            foreach (LocalizedDataConfig config in localizedDataHandle)
            {
                if (config.LanguageName == CurrentLang)
                {
                    // 
                    string path = Paths.CombinePathWithStreamingAssetsPath(config.LanguageKeyValueArrayPath);
                    if (ConsoleCat.Enable)
                        ConsoleCat.Log("正从该路径中查找语言：" + path);
                    SerializationUniJsonSafe.SingleFolderMultipleFiles(typeof(LanguageKeyValueArray), Paths.searchType_Json, PopulateLanguages, path);
                }
            }
        }
        /// <summary>
        /// 填充字典
        /// </summary>
        void PopulateLanguages(object obj)
        {
            if (obj is LanguageKeyValueArray kvA)
            {
                StringKV[] keyValues = kvA.StringKVs;
                for (int k = 0; k < keyValues.Length; k++)
                {
                    if (keyValues[k].Key == null)
                    {
                        ConsoleCat.LogWarning("译文键值对有空键");
                        continue;
                    }
                    if (languageDic.TryGetValue(keyValues[k].Key, out Language language))
                        language.SetContent(keyValues[k].Value);
                    else
                        languageDic[keyValues[k].Key] = new Language(keyValues[k].Key, keyValues[k].Value);
                }
            }
        }
        public class LocalizedDataConfigs : IEnumerable<LocalizedDataConfig>
        {
            List<LocalizedDataConfig> configs;
            public LocalizedDataConfigs()
            {
                configs = new List<LocalizedDataConfig>();
                string name = "LocalizedDataConfig.json";
                SerializationUniJsonSafe.SingleFile(out object data, typeof(LocalizedDataConfig), Application.streamingAssetsPath, name);
                if (data is LocalizedDataConfig config)
                {
                    Add(config);
                }
                SerializationUniJsonSafe.MultipleChildrenFoldersSameFile(typeof(LocalizedDataConfig), Populate, Paths.ModsFolderPath, name);
            }
            void Populate(object data, string folderPath)
            {
                if (data is LocalizedDataConfig config)
                {
                    Add(config);
                }
            }
            public void Clear()
            {
                configs.Clear();
            }
            public void Add(LocalizedDataConfig config)
            {
                configs.Add(config);
            }
            public IEnumerator<LocalizedDataConfig> GetEnumerator()
            {
                return configs.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return configs.GetEnumerator();
            }
        }
    }
    // 自身一个，模组每个一个
    [Serializable]
    public class LocalizedDataConfig
    {
        [SerializeField] string name;
        [SerializeField] string version;
        [SerializeField] string author;
        [SerializeField] string description;
        [SerializeField] string languageName;
        [SerializeField] string languageKeyValueArrayPath;
        public string Name => name;
        public string Version => version;
        public string Author => author;
        public string Description => description;
        public string LanguageName => languageName;
        public string LanguageKeyValueArrayPath => languageKeyValueArrayPath;
    }
    [Serializable]
    public class LanguageKeyValueArray
    {
        [SerializeField] StringKV[] keyValues;
        public StringKV[] StringKVs => keyValues;
    }
    /// <summary>
    /// languageName folderPath
    /// </summary>
    [Serializable]
    public struct LanguageConfig
    {
        [SerializeField] string languageName;
        [SerializeField] string folderPath;
        public readonly string LanguageName => languageName;
        /// <summary>
        /// 从流文件夹之下开始的文件夹路径
        /// </summary>
        public readonly string FolderPath => folderPath;
    }
    /// <summary>
    /// key value
    /// </summary>
    [Serializable]
    public struct StringKV
    {
        [SerializeField] string key;
        [SerializeField] string value;
        public readonly string Key => key;
        public readonly string Value => value;
    }
}
