using UnityEngine;

public static class Vector3Extensions
{
    // ReSharper disable once InconsistentNaming
    public static Vector2 XZToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    // ReSharper disable once InconsistentNaming
    public static Vector2 XYToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector3 ZeroY(this Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }

    public static Vector3 NewY(this Vector3 vector, float y)
    {
        return new Vector3(vector.x, y, vector.z);
    }
}
