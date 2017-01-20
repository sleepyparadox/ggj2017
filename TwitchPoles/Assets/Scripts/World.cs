using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public static class World
    {
        const float HopFrequency = 20f;
        const float HopHeight = 20f;

        public readonly static Vector3 Size;

        static World()
        {
            Size = new Vector3(800, 600, 600);
        }

        public static Vector3 ArenaToWorld(Vector3 arenaPos, float seed)
        {
            if(arenaPos.y == 0f)
            {
                arenaPos.y += Mathf.Sin((Time.time + seed) * HopFrequency) * 10f;
            }

            arenaPos.y += arenaPos.z * 0.5f;
            return arenaPos;
        }
    }
}
