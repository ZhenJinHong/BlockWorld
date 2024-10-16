using System.Runtime.CompilerServices;

namespace CatDOTS.VoxelWorld
{
    // 千个形状约1MB:4B*6Int*7Face*8ShapeVariant*1000个
    /// <summary>
    /// 当添加一个面的时候,需要加顶点开始索引直到结束的所有顶点,然后也加入所有的该面的索引(0-N)
    /// 即加入一个分段,
    /// </summary>
    /// <remarks>
    /// 定义顺序为前面(顺Z轴),背面,上面,下面,右面,左面; 
    /// 每个面的索引都应该从0重新开始,当绘制该面时,索引的偏移是从正在构建的网格的顶点数组长度确定的,而不是由这个面数据的(0-N)索引决定的
    /// </remarks>
    public struct VoxelFaceData
    {
        /// <summary>
        /// 每个方向变体的面数量,包括非面
        /// </summary>
        public const int FaceCountInSingleShape = 7;
        /// <summary>
        /// 索引分段开始
        /// </summary>
        public int IndexStartIndex;
        /// <summary>
        /// 索引分段结束(不含自身)
        /// </summary>
        public int IndexEnd;
        /// <summary>
        /// 顶点分段开始
        /// </summary>
        public int VertexStartIndex;
        /// <summary>
        /// 顶点分段结束(不含自身)
        /// </summary>
        public int VertexEnd;
        //public byte OffsetRect;
        //public byte Placeholder1;
        //public byte Placeholder2;
        //public byte Placeholder3;
        public FaceRect FaceRect;
    }
}
