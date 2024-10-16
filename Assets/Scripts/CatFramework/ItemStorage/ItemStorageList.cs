using System;
using System.Collections.Generic;

namespace CatFramework
{
    // 要注意的,我的目的是提供一个当修改了集合的数据后,集合可以通知所有的内容为该集合的视图去更新自身
    public class ItemStorageList<T> : List<T>, IReadonlyItemStorageCollection<T>
    {
        public ItemStorageList() { }
        public ItemStorageList(int capacity) : base(capacity) { }
        public int CurrentSelectedIndex { get; set; }
        public T GetItem(int index)
        {
            return this[index];
        }

        public bool IndexInRange(int targetIndex)
        {
            return targetIndex > -1 && targetIndex < Count;
        }

        public bool IndexIsEmpty(int targetIndex)
        {
            return this[targetIndex] == null;
        }
    }
}
