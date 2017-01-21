using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public enum Team
    {
        Left,
        Right,
    }

    public static class TeamUtility
    {
        public static float GetArenaXStart(this Team team)
        {
            if (team == Team.Left)
                return 0;

            if (team == Team.Right)
                return ArenaTransformer.Size.x;

            throw new NotImplementedException();
        }
        public static Vector3 GetDirection(this Team team)
        {
            if (team == Team.Left)
                return Vector3.right;

            if (team == Team.Right)
                return Vector3.left;

            throw new NotImplementedException();
        }
    }
}
