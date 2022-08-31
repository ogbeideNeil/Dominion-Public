using UnityEngine;

public static class Vector2Extensions
{
    // ReSharper disable once InconsistentNaming
    public static Vector3 ToVector3XZ(this Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }

    // ReSharper disable once InconsistentNaming
    public static Vector3 ToVector3XZ(this Vector2 vector, float y)
    {
        return new Vector3(vector.x, y, vector.y);
    }

    public static Vector2 ClosestPointToCentre(this Vector2 vector, Vector2 centre, float radius)
    {
        Vector2 offset = vector - centre;

        return vector + (radius * (-offset / offset.magnitude));
    }
}