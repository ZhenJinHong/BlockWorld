using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CatFramework.CatMath
{
    public static class MathC_2DGridPlane
    {
        public static bool GetPointInPlane(Ray ray, out Vector3 point)
        {
            Plane plane = new Plane(new Vector3(0f, 1f, 0f), 0f);
            if (plane.Raycast(ray, out float enter))
            {
                point = ray.GetPoint(enter);
                return true;
            }
            point = default;
            return false;
        }
        public static Vector3 PointAlignGrid(Vector3 point)
        {
            return MathC.Floor(point) + new Vector3(0.5f, -point.y + 0.5f, 0.5f);
        }
    }
}
