using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ResourceWorld", fileName = "ResourceWorld", order = 0)]
public class ResourceWorld : ScriptableObject
{
    public GameObject DropPrefab;
    public eCollectable TypeWallet;
    public float Health;
    [Range(1, 5)] public int VisualDrop;
    
}