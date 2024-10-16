using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CatFramework
{
    /// <summary>
    /// 管理器不要引用其他管理器里的数据，除非管理器可以单独持有该其他管理器
    /// </summary>
    /// <remarks>
    /// 对于在某个方法内需要频繁调用静态管理器的，使用临时变量去引用，是有必要的
    /// 因为静态管理有的一个静态实例在使用的时候是频繁判空的，这导致循环这类的方式无法内联（可能是这个原因）该静态变量
    /// </remarks>
    public class Manager
    {
        protected bool m_IsShutDown;
        public bool IsShutDown => m_IsShutDown;
        protected internal virtual void Start() { }
        //public virtual void Update() { }
        //public virtual void LateUpdate() { }
        public virtual void DebugInfo() { }
        protected internal virtual void ShutDown() { }
    }
    ///// <summary>
    ///// 所以被静态直接调用的字段或属性，都按照属性写法，后跟_I（如：Value_I），代表实例或内部
    ///// </summary>
    ///// <remarks>
    ///// 任何时候实例管理器不准调用静态方法
    ///// 应当由静态方法调用实例管理器（管理器实际上就是个存数据的类）
    ///// </remarks>
    //public class Manager<T> : Manager where T : Manager, new()
    //{
    //    static T instance;
    //    public static T Instance { get { instance ??= Entry.GetManager<T>(); return instance; } }
    //    protected static bool IsShutDown => Instance.IsShutDown_I;
    //    public override void ShutDown()
    //    {
    //        base.ShutDown();
    //        instance = null;// 置空不代表实例丢失，这个依旧在Entry里，并再次被外界需要时被重新赋值
    //    }
    //}
}