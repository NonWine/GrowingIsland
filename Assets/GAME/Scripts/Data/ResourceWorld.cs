using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ResourceWorld", fileName = "ResourceWorld", order = 0)]
public class ResourceWorld : ScriptableObject
{
    public GameObject DropPrefab;
    public eCollectable TypeWallet;
    public float Health;
    [Min(0f)] public float RespawnTime = 10f;
    [Range(1, 100)] public int VisualDrop;
}
