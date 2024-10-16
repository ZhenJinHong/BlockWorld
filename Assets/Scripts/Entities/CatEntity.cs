using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework.CatEntity
{
    public interface ICatEntity
    {
        T GetComponent<T>() where T : class, IComponent;
        bool TryGetComponent<T>(out T component) where T : class, IComponent;
    }
    public interface IComponent
    {
        ICatEntity CatEntity { get; }
    }
    //public class Component : IComponent
    //{
    //    public ICatEntity CatEntity { get; set; }
    //    public Component()
    //    {

    //    }
    //    public Component(ICatEntity catEntity)
    //    {

    //    }

    //    public T GetComponent<T>() where T : class, IComponent
    //    {
    //        return CatEntity.GetComponent<T>();
    //    }
    //}
    //public class CatEntity : ICatEntity
    //{
    //    public T GetComponent<T>()
    //    {
    //        if (this is T component)
    //            return component;
    //        return default;
    //    }

    //    public bool TryGetComponent<T>(out T component)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
