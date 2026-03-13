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
    public float DropDetectionRadius = 25f;
    public float RetargetCooldown = 0.5f;
    public float TreeDamage;
    public float ChopInterval = 1.5f;
    public int CarryCapacity = 5;

    public DepositAnimationSettings DepositAnimation = new();

    public WoodcutterWorkSettings() { }

    public WoodcutterWorkSettings(WoodcutterWorkSettings template)
    {
        ResourceMask = template.ResourceMask;
        ResourcePartMask = template.ResourcePartMask;
        TreeSearchRadius = template.TreeSearchRadius;
        ChopDistance = template.ChopDistance;
        DepositDistance = template.DepositDistance;
        DropDetectionRadius = template.DropDetectionRadius;
        RetargetCooldown = template.RetargetCooldown;
        TreeDamage = template.TreeDamage;
        ChopInterval = template.ChopInterval;
        CarryCapacity = template.CarryCapacity;
        DepositAnimation = new DepositAnimationSettings(template.DepositAnimation);
    }
}
