//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Unity.Entities;
//using Unity.Jobs;

//namespace CatDOTS.VoxelWorld
//{
//    public class TreeEntityFiller : IEntityFiller
//    {
//        public string Name { get; set; }
//        public Entity Toput;
//        public float RangeScale;
//        public float PutScale;
//        public float RangeThreshold;
//        public float PutThreshold;
//        public JobHandle Schedule(VoxelWorldMap.BigChunkSliceReadOnly bigChunkSlice, EntityCommandBuffer ECB, JobHandle dependOn)
//        {
//            return new TreeEntityFillJob()
//            {
//                BigChunkSlice = bigChunkSlice,
//                ECB = ECB,
//                Toput = Toput,
//                RangeScale = RangeScale,
//                PutScale = PutScale,
//                RangeThreshold = RangeThreshold,
//                PutThreShold = PutThreshold,
//            }.Schedule(dependOn);
//        }

//        public void Dispose()
//        {
//        }
//    }
//}
