using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct Vec3
{
    public int x;
    public int y;
    public int z;

    public Vec3(int argX = 0, int argY = 0, int argZ = 0)
    {
        x = argX;
        y = argY;
        z = argZ;
    }

    public static Vec3 Right { get { return new Vec3(1, 0, 0); } }
    public static Vec3 Left { get { return new Vec3(-1, 0, 0); } }
    public static Vec3 Up { get { return new Vec3(0, 1, 0); } }
    public static Vec3 Down { get { return new Vec3(0, -1, 0); } }
    public static Vec3 Forward { get { return new Vec3(0, 0, 1); } }
    public static Vec3 Back { get { return new Vec3(0, 0, -1); } }
    public static Vec3 One { get { return new Vec3(1, 1, 1); } }
    public static Vec3 Zero { get { return new Vec3(0, 0, 0); } }

    public static bool operator ==(Vec3 a, Vec3 b)
    {
        return a.x == b.x
                && a.y == b.y
                && a.z == b.z;
    }

    public static bool operator !=(Vec3 a, Vec3 b)
    {
        return a.x != b.x
                || a.y != b.y
                || a.z != b.z;
    }

    public override bool Equals(object obj)
    {
        return obj is Vec3 && (Vec3)obj == this;
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
    }

    [Obsolete("Used for dictionary sorting only")]
    public static bool operator <(Vec3 a, Vec3 b)
    {
        return a.x < b.x
                && a.y < b.y
                && a.z < b.z;
    }

    [Obsolete("Used for dictionary sorting only")]
    public static bool operator >(Vec3 a, Vec3 b)
    {
        return a.x > b.x
                && a.y > b.y
                && a.z > b.z;
    }
}
