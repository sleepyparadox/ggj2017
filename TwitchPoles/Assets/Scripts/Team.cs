using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public enum Team
    {
        lower,
        UPPER,
    }

    public static class TeamUtility
    {
        public static string ToTeam(this string src, Team team)
        {
            if (team == Team.lower)
                return src.ToLower();

            if (team == Team.UPPER)
                return src.ToUpper();

            throw new NotImplementedException();
        }
        public static float GetArenaXStart(this Team team)
        {
            if (team == Team.lower)
                return 0;

            if (team == Team.UPPER)
                return Arena.Width - 1;

            throw new NotImplementedException();
        }
        public static Vector3 GetDirection(this Team team)
        {
            if (team == Team.lower)
                return Vector3.right;

            if (team == Team.UPPER)
                return Vector3.left;

            throw new NotImplementedException();
        }
    }
}
