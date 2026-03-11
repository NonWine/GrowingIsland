using System;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineProperty, Serializable, HideLabel]
public class WoodcutterWorkSettings
{
    [SerializeField] public LayerMask ResourceMask = ~0;
    [SerializeField] public LayerMask ResourcePartMask = ~0;
    public float TreeSearchRadius = 20f;
    public float ChopDistance = 1.6f;
    public float DepositDistance = 1.5f;
    public float LootCollectionRadius = 3f;
    public float DropDetectionRadius = 25f;
    public float RetargetCooldown = 0.5f;
    public float TreeDamage;
    public int WoodPerHit = 1;
    
    public WoodcutterWorkSettings() { }

    public WoodcutterWorkSettings(WoodcutterWorkSettings template)
    {
        ResourceMask = template.ResourceMask;
        ResourcePartMask = template.ResourcePartMask;
        TreeSearchRadius = template.TreeSearchRadius;
        ChopDistance = template.ChopDistance;
        DepositDistance = template.DepositDistance;
        LootCollectionRadius = template.LootCollectionRadius;
        DropDetectionRadius = template.DropDetectionRadius;
        RetargetCooldown = template.RetargetCooldown;
        TreeDamage = template.TreeDamage;
        WoodPerHit = template.WoodPerHit;
    }


}