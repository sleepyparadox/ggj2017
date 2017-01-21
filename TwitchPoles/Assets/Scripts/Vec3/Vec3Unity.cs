using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Vec3Unity
    {
        public static Vector3 ToVector3(this Vec3 src)
        {
            return new Vector3(src.x, src.y, src.z);
        }

        public static Vec3 ToVec3(this Vector3 src)
        {
            return new Vec3((int)src.x, (int)src.y, (int)src.z);
        }
    }
}
