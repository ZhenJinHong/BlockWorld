using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework.Localized
{
    public static class LocalizedManagerMiao
    {
        class Events
        {
            public Action LanguageChange;
        }
        static LanguageCollection languageCollection;
        static object locker = new object();
        public static LanguageCollection LanguageCollection
        {
            get
            {
                if (languageCollection == null)
                {
                    lock (locker)
                    {
                        languageCollection ??= new LanguageCollection();
                    }
                }
                return languageCollection;
            }
        }
        static Events events;
        public static event Action OnLanguageChange
        {
            add { events.LanguageChange += value; }
            remove { events.LanguageChange -= value; }
        }
        //public static void NotifyLanguageChange()
        //{
        //    events.LanguageChange?.Invoke();
        //}
        public static void SwitchLanguage(string languageName)
        {
            if (LanguageCollection.SwitchLanguageWithoutNotify(languageName))
            {
                events.LanguageChange?.Invoke();
                LanguageCollection.Save();
            }
        }
        static LocalizedManagerMiao()
        {
            events = new Events();
        }
        public static void Start()
        {

        }
        public static void Shutdown()
        {
            events = new Events();
        }
    }
}
