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
        public static Color ToColor(this Team team, bool isTextMesh)
        {
            var color = Color.white;
            if (team == Team.lower)
                color = Color.red;

            if (team == Team.UPPER)
                color = Color.blue;

            if (isTextMesh)
                color = Color.Lerp(color, Color.black, 0.75f);

            return color;
        }
        public static Team GetOpponent(this Team team)
        {
            if (team == Team.lower)
                return Team.UPPER;

            if (team == Team.UPPER)
                return Team.lower;

            throw new NotImplementedException();
        }
        public static string ToTeam(this string src, Team team)
        {
            if (team == Team.lower)
                return src.ToLower();

            if (team == Team.UPPER)
                return src.ToUpper();

            throw new NotImplementedException();
        }
        public static int GetArenaXStart(this Team team)
        {
            if (team == Team.lower)
                return 0;

            if (team == Team.UPPER)
                return Arena.Width -1;

            throw new NotImplementedException();
        }

        public static int GetWordXStart(this Team team)
        {
            if (team == Team.lower)
                return (int)(Arena.Width * 0.31f);

            if (team == Team.UPPER)
                return (int)(Arena.Width * 0.69f);

            throw new NotImplementedException();
        }
        public static Vec3 GetDirection(this Team team)
        {
            if (team == Team.lower)
                return Vec3.Right;

            if (team == Team.UPPER)
                return Vec3.Left;

            throw new NotImplementedException();
        }
    }
}
