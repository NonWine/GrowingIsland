using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerStats : Stats
{
    [FormerlySerializedAs("_axeDamage")]
    [SerializeField] private float axeDamage;
    [FormerlySerializedAs("_pickAxeDamage")]
    [SerializeField] private float pickAxeDamage;
    [FormerlySerializedAs("_digDamage")]
    [SerializeField] private float digDamage;
    [FormerlySerializedAs("_damage")]
    [SerializeField] private float damage;
    [FormerlySerializedAs("_coolDown")]
    [SerializeField] private float coolDown;
    [FormerlySerializedAs("_maxEnemyDetection")]
    [SerializeField] private int maxEnemyDetection;
    
    [field: SerializeField] public float perHitRadius  { get; private set; }
    [field: SerializeField] public float RadiusFarming { get; private set; }

    [field: SerializeField]
    public float MiningCD
    {
        get; private set;
    }
    public float Damage => damage;
    
    
    public float CoolDown => coolDown;

    public int MaxEnemyDetection => maxEnemyDetection;

    
    public float AxeDamage => axeDamage;

    public float PickAxeDamage => pickAxeDamage;

    public float DigDamage => digDamage;
}
