using UnityEngine;

[System.Serializable]
public class PlayerStats : Stats
{
    [SerializeField] private float _axeDamage;
    [SerializeField] private float _pickAxeDamage;
    [SerializeField] private float _digDamage;
    [SerializeField] private float _damage;
    [SerializeField] private float _coolDown;
    [SerializeField] private int _maxEnemyDetection;

    [field: SerializeField] public float RadiusFarming { get; private set; }

    [field: SerializeField]
    public float MiningCD
    {
        get; private set;
    }
    public float Damage => _damage;
    
    
    public float CoolDown => _coolDown;

    public int MaxEnemyDetection => _maxEnemyDetection;

    
    public float AxeDamage => _axeDamage;

    public float PickAxeDamage => _pickAxeDamage;

    public float DigDamage => _digDamage;
}