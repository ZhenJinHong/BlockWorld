using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CatFramework
{
    public readonly struct ManagedObjectRef<T> where T : class
    {
        public readonly int Id;
        public ManagedObjectRef(int id)
        {
            Id = id;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(ManagedObjectRef<T> objectRef)
            => objectRef.Id;
    }
    public class ManagedObjectWorld<T>
        where T : class
    {
        Stack<int> emptySeats;
        List<T> objs;
        public ManagedObjectWorld()
        {
            emptySeats = new Stack<int>();
            objs = new List<T>();
        }
        public T Get(ManagedObjectRef<T> objectRef)
        {
            return objs[objectRef.Id];
        }
        public ManagedObjectRef<T> Add(T obj)
        {
            if (object.Equals(obj, null)) throw new NullReferenceException(this.GetType().FullName + "不可以添加空对象");
            int id;
            if (emptySeats.Count == 0)
            {
                id = objs.Count;
                objs.Add(obj);
            }
            else
            {
                id = emptySeats.Pop();
                objs[id] = obj;
            }
            return new ManagedObjectRef<T>(id);
        }
        public void Remove(ManagedObjectRef<T> objectRef)
        {
            int id = objectRef.Id;
            T obj = objs[id];
            // 判断非空,即未被移除过
            if (!object.Equals(obj, null))// 避免掉Unity对象的判空方式,销毁的Unity对象==null(此时会导致无法移除)
            {
                emptySeats.Push(id);// 添加空位
                objs[id] = null;
            }
            else
            {
                if (ConsoleCat.Enable)
                    ConsoleCat.LogWarning("重复移除托管引用");
            }
        }
    }
}
