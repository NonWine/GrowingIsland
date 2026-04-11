using UnityEngine;

public static class RandomExtensions
{
    public static Vector3 RandomHorizontalOffset(float radius)
    {
        if (radius <= 0f)
        {
            return Vector3.zero;
        }

        Vector2 offset2D = Random.insideUnitCircle * radius;
        return new Vector3(offset2D.x, 0f, offset2D.y);
    }
}