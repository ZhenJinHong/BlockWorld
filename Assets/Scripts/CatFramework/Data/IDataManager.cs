using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework
{
    public interface IDataManager
    {
        void AddDataCollection<T>(string key, T value) where T : class, IDataCollection;
        void RemoveDataCollection<T>(string key) where T : class, IDataCollection;
    }
    public interface IDataProvider
    {
        //void AddListen<T>(string key, Action<object> action) where T : class;
        //void RemoveListen<T>(string key, Action<object> action) where T : class;
        T GetDataCollection<T>(string key) where T : class, IDataCollection;
        bool TryGetDataCollection<T>(string key, out T dataCollection) where T : class, IDataCollection;
    }
}
