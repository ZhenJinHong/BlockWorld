using CatFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace CatDOTS.VoxelWorld
{
    public class BigChunkManaged
    {
        public const int AllMeshDirty = int.MaxValue;// 0b1111111111111111111111111111111 // 31个1，要加符号得在0前
        VoxelWorldMap.BigChunkSlice slice;
        public readonly int3 BigChunkIndex;// 保留一个自己的否则会导致在解除切片的时候，拿到错误的索引
        public ref VoxelWorldMap.BigChunkSlice Slice => ref slice;// 不要把切片放出去允许更改，这个bigChunk卸载的时候直接丢掉
        // 整个大区块但凡变更一个小的，整个都需要重新保存
        bool dirty;
        /// <summary>
        /// 脏区块卸载时要保存
        /// </summary>
        public bool Dirty => dirty;
        public BigChunkManaged(VoxelWorldMap.BigChunkSlice slice)
        {
            this.slice = slice;
            BigChunkIndex = slice.BigChunkIndex;
            if (BigChunkIndex.y != 0 && ConsoleCat.Enable)
            {
                ConsoleCat.LogWarning($"出现越界大区块：{BigChunkIndex}");
            }
            // 不需要在新生成的区块设置脏网格，默认新生成就要加入生成网格
        }
        /// <summary>
        /// 后续看情况，还需要一个手动保存的，则需要取消Dirty
        /// </summary>
        public void SetDirty()
        {
            dirty = true;
        }
        public void ClearSlice()
        {
            slice = default;
        }
    }
}
