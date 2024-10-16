using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFramework
{
    // 格子,储存物品的类
    // 格子:显示储存的物品,至于这个格子能否放置某物,由操作模块决定,再小的集合比如魔杖,也是一个库存,
    // 操作模块:如果模块可以知道库存允许类型,也可以不知道(全部该成知道的?),这个时候就需要访问库存确定类型

    // model:数据,view:视图,ctr:控制->数据->ctr->视图,视图->ctr->数据?
    // 不加序列化方法,因为不是所有物品需要保存,比如蓝图就不用保存
    public interface IItemStorage
    {
    }
    //  特征可合并
    public interface ITraitsMergeable
    {
        bool TryOtherItemDrop(IItemStorage item);
    }
    public interface IMergedable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storage"></param>
        /// <remarks>大多情况下不需要使用,如果需要合并,直接使用合并</remarks>
        bool TypeIsMatch(IItemStorage storage);
    }
    public interface IMergedable<T> : IMergedable
        where T : IItemStorage
    {
        bool TryMerged(T item, out T ret);
    }
}
