using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace CatDOTS
{
    public struct FirstPersonPlayer : IComponentData
    {
        public float TopAngle;
        public float BottomAngle;
        //public float RotateChangeSpeed;
        //public float OverLoadFactor;
        //public float JumpVelocity;
        //public float MoveSpeed;
    }
}
