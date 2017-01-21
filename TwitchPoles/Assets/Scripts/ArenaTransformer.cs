using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public static class ArenaTransformer
    {
        const float HopFrequency = 20f;
        const float HopHeight = 3f;

        public readonly static Vector3 Size;

        static ArenaTransformer()
        {
            Size = new Vector3(800, 600, 600);
        }

        public static Vector3 ArenaToWorld(Vector3 arenaPos, float seed)
        {
            var worldPos = arenaPos;
            worldPos.x = Mathf.Floor(arenaPos.x);
            worldPos.z = Mathf.Floor(arenaPos.z);

            worldPos.y = Mathf.Floor(worldPos.y) + worldPos.z;

            worldPos *= 8;

            if (arenaPos.y == 0f)
            {
                worldPos.y += (int)(Mathf.Sin((Time.time + seed) * HopFrequency) * HopHeight);
            }


            // Sprite offset
            worldPos.x += 4;
            worldPos.y += 4;

            return worldPos;
        }
    }
}
