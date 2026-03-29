using UnityEngine;

public static class AnimationEasing
{
    public static float Normalize01(float time, float duration)
    {
        if (duration <= 0f)
        {
            return 1f;
        }

        return Mathf.Clamp01(time / duration);
    }

    public static float EaseOutQuad(float t)
    {
        return 1f - (1f - t) * (1f - t);
    }

    public static float EaseInOutQuad(float t)
    {
        return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) * 0.5f;
    }

    public static float EaseOutCubic(float t)
    {
        var inv = 1f - t;
        return 1f - inv * inv * inv;
    }
}
