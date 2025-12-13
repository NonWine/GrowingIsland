using UnityEngine;

[System.Serializable]
public class TreeHitAnimationSettings
{
    public float LeanAngle = 10f;
    public float HitPushDuration = 0.14f;
    public float ReturnDuration = 0.26f;
    public float ScaleBump = 1.02f;
    public float ReboundElasticity = 0.25f;
    public float ReboundPeriod = 0.08f;
    public float ShakeDuration = 0.18f;
    public Vector3 ShakeStrength = new Vector3(2f, 0.3f, 2f);
    public int ShakeVibrato = 12;
    public float ShakeRandomness = 80f;
}
