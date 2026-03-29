using UnityEngine;

[CreateAssetMenu(menuName = "AnimationData/ResourceData", fileName = "ResourceData", order = 0)]

public class ResourceData : AnimationData
{
    public int CountResourceInAnimation;
    public Quaternion StartRotation = Quaternion.Euler(new Vector3(-30f, 0f, 0f));
    public float JumpPower = 2f;
    public int NumJumps = 1;
    public float DelayPerResource = 0.05f;
}