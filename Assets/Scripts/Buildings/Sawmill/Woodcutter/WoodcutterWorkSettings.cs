using UnityEngine;

[CreateAssetMenu(fileName = "WoodcutterWorkSettings", menuName = "ScriptableObjects/WoodcutterWorkSettings", order = 1)]
public class WoodcutterWorkSettings : ScriptableObject
{
    [SerializeField] public LayerMask ResourceMask = ~0;
    [SerializeField] public LayerMask ResourcePartMask = ~0;
    public float TreeSearchRadius = 20f;
    public float ChopDistance = 1.6f;
    public float DepositDistance = 1.5f;
    public float LootCollectionRadius = 3f;
    public float DropDetectionRadius = 25f;
    public float RetargetCooldown = 0.5f;
    public float TreeDamage = 1f;
    public int WoodPerHit = 1;
}