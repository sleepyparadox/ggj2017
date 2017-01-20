using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public enum Team
    {
        East,
        West
    }

    public static class TeamUtility
    {
        public static float GetArenaXStart(this Team team)
        {
            if (team == Team.East)
                return 0;

            if (team == Team.West)
                return World.Size.x;

            throw new NotImplementedException();
        }
        public static Vector3 GetDirection(this Team team)
        {
            if (team == Team.East)
                return Vector3.right;

            if (team == Team.West)
                return Vector3.left;

            throw new NotImplementedException();
        }
    }
}
