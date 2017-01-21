using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Vec3Ray
{
    public static IEnumerable<Vec3> GetAllHit(Vec3 src, Vec3 dst)
    {
        var dtX = (double)dst.x - src.x;
        var dtY = (double)dst.y - src.y;
        var dtZ = (double)dst.z - src.z;
        var startX = src.x + 0.5;
        var startY = src.y + 0.5;
        var startZ = src.z + 0.5;


        if (Math.Abs(dtX) >= Math.Abs(dtY)
            && Math.Abs(dtX) >= Math.Abs(dtZ))
        {
            var scale = Math.Abs(dtX);
            var mY = dtY / scale;
            var mZ = dtZ / scale;

            var diagonalY = Math.Abs(mY) == 1;
            var diagonalZ = Math.Abs(mZ) == 1;


            var iIsAcending = src.x < dst.x;
            var dY = (double)src.y + 0.5;
            var dZ = (double)src.z + 0.5;

            int oldY = int.MinValue;
            int oldZ = int.MinValue;

            for (var x = src.x; iIsAcending ? x <= dst.x : x >= dst.x; x += iIsAcending ? 1 : -1)
            {
                var y = (int)Math.Floor(dY);
                var z = (int)Math.Floor(dZ);

                if (!diagonalZ
                    && z != oldZ
                    && x != src.x)
                {
                    var zAtMiddle = (int)Math.Floor(dZ - (mZ / 2f));
                    if (zAtMiddle == z)
                        yield return new Vec3(x + (iIsAcending ? -1 : +1), y, z);
                    else
                        yield return new Vec3(x, y, oldZ);
                }

                if (!diagonalY
                    && y != oldY
                    && x != src.x)
                {
                    var yAtMiddle = (int)Math.Floor(dY - (mY / 2f));
                    if (yAtMiddle == y)
                        yield return new Vec3(x + (iIsAcending ? -1 : +1), y, z);
                    else
                        yield return new Vec3(x, oldY, x);
                }

                yield return new Vec3(x, y, z);

                dY += mY;
                dZ += mZ;

                oldY = y;
                oldZ = z;
            }
        }
        else if (Math.Abs(dtZ) >= Math.Abs(dtY)
            && Math.Abs(dtZ) >= Math.Abs(dtX))
        {
            var scale = Math.Abs(dtZ);
            var mY = dtY / scale;
            var mX = dtX / scale;

            var diagonalY = Math.Abs(mY) == 1;
            var diagonalX = Math.Abs(mX) == 1;

            var iIsAcending = src.z < dst.z;
            var dY = (double)src.y + 0.5;
            var dX = (double)src.x + 0.5;

            int oldY = int.MinValue;
            int oldX = int.MinValue;

            for (var z = src.z; iIsAcending ? z <= dst.z : z >= dst.z; z += iIsAcending ? 1 : -1)
            {
                var y = (int)Math.Floor(dY);
                var x = (int)Math.Floor(dX);

                if (!diagonalX
                    && x != oldX
                    && z != src.z)
                {
                    var xAtMiddle = (int)Math.Floor(dX - (mX / 2f));
                    if (xAtMiddle == x)
                        yield return new Vec3(x, y, z + (iIsAcending ? -1 : +1));
                    else
                        yield return new Vec3(oldX, y, z);
                }

                if (!diagonalY
                    && y != oldY
                    && z != src.z)
                {
                    var yAtMiddle = (int)Math.Floor(dY - (mY / 2f));
                    if (yAtMiddle == y)
                        yield return new Vec3(x, y, z + (iIsAcending ? -1 : +1));
                    else
                        yield return new Vec3(x, oldY, z);
                }

                yield return new Vec3(x, y, z);

                dY += mY;
                dX += mX;

                oldY = y;
                oldX = x;
            }
        }
        else
        {
            var scale = Math.Abs(dtY);
            var mX = dtX / scale;
            var mZ = dtZ / scale;

            var diagonalX = Math.Abs(mX) == 1;
            var diagonalZ = Math.Abs(mZ) == 1;

            var iIsAcending = src.y < dst.y;
            var dX = (double)src.x + 0.5;
            var dZ = (double)src.z + 0.5;

            int oldX = int.MinValue;
            int oldZ = int.MinValue;

            for (var y = src.y; iIsAcending ? y <= dst.y : y >= dst.y; y += iIsAcending ? 1 : -1)
            {
                var x = (int)Math.Floor(dX);
                var z = (int)Math.Floor(dZ);

                if (!diagonalZ
                    && z != oldZ
                    && y != src.y)
                {
                    var zAtMiddle = (int)Math.Floor(dZ - (mZ / 2f));
                    if (zAtMiddle == z)
                        yield return new Vec3(x, y + (iIsAcending ? -1 : +1), z);
                    else
                        yield return new Vec3(x, y, oldZ);
                }

                if (!diagonalX
                    && x != oldX
                    && y != src.y)
                {
                    var xAtMiddle = (int)Math.Floor(dX - (mX / 2f));
                    if (xAtMiddle == y)
                        yield return new Vec3(x, y + (iIsAcending ? -1 : +1), z);
                    else
                        yield return new Vec3(oldX, y, z);
                }

                yield return new Vec3(x, y, z);

                dX += mX;
                dZ += mZ;

                oldX = x;
                oldZ = z;
            }
        }
    }
    public static IEnumerable<Vec3> GetLine(Vec3 src, Vec3 dst)
    {
        var dtX = (double)dst.x - src.x;
        var dtY = (double)dst.y - src.y;
        var dtZ = (double)dst.z - src.z;

        if (Math.Abs(dtX) > Math.Abs(dtY)
            && Math.Abs(dtX) > Math.Abs(dtZ))
        {
            var scale = Math.Abs(dtX);
            dtY /= scale;
            dtZ /= scale;

            var iIsAcending = src.x < dst.x;
            var y = (double)src.y;
            var z = (double)src.z;

            var isDiagonal = Math.Abs(dtY) == Math.Abs(dtZ);

            for (var x = src.x; iIsAcending ? x < dst.x : x > dst.x; x += iIsAcending ? 1 : -1)
            {
                y += dtY;
                z += dtZ;

                yield return new Vec3(x, (int)Math.Round(y), (int)Math.Round(z));
            }
        }
        else if (Math.Abs(dtY) > Math.Abs(dtX)
            && Math.Abs(dtY) > Math.Abs(dtZ))
        {
            var scale = Math.Abs(dtY);
            dtX /= scale;
            dtZ /= scale;

            var iIsAcending = src.y < dst.y;
            var x = (double)src.x;
            var z = (double)src.z;

            for (var y = src.y; iIsAcending ? y < dst.y : y > dst.y; y += iIsAcending ? 1 : -1)
            {
                x += dtX;
                z += dtZ;

                yield return new Vec3((int)Math.Round(x), y, (int)Math.Round(z));
            }
        }
        else //if (Math.Abs(dtZ) > Math.Abs(dtX)
        //&& Math.Abs(dtZ) > Math.Abs(dtY))
        {
            var scale = Math.Abs(dtZ);
            dtX /= scale;
            dtY /= scale;

            var iIsAcending = src.z < dst.z;
            var x = (double)src.x;
            var y = (double)src.y;

            for (var z = src.z; iIsAcending ? z < dst.z : z > dst.z; z += iIsAcending ? 1 : -1)
            {
                x += dtX;
                y += dtY;

                yield return new Vec3((int)Math.Round(x), (int)Math.Round(y), z);
            }
        }
    }
}
