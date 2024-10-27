using System;
using UnityEngine;

[System.Serializable]
public class Stats
{
    [SerializeField] private float _health;
    [SerializeField] private int _moveSpeed;
    [SerializeField] private int _roateSpeed;
    private float _currentHealth;
    private bool _isDeath;

    public event Action<float> OnHealthChange;
    
    public int MoveSpeed => _moveSpeed;
    
    public int RotateSpeed => _moveSpeed;

    public float MaxHealth => _health;
    
    public float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            OnHealthChange?.Invoke(_currentHealth);
        }
    }

    public bool IsDeath
    {
        get => _isDeath;
        set => _isDeath = value;
    }



}
[System.Serializable]
public class PlayerStats : Stats
{
    [SerializeField] private float _axeDamage;
    [SerializeField] private float _pickAxeDamage;
    [SerializeField] private float _digDamage;
    [SerializeField] private float _damage;
    [SerializeField] private float _coolDown;
    [SerializeField] private int _maxEnemyDetection;
    [SerializeField] private float _radiusDetection;

    [field: SerializeField]
    public float MiningCD
    {
        get; private set;
    }
    public float Damage => _damage;
    
    
    public float CoolDown => _coolDown;

    public int MaxEnemyDetection => _maxEnemyDetection;

    public float RadiusDetection => _radiusDetection;
    
    public float AxeDamage => _axeDamage;

    public float PickAxeDamage => _pickAxeDamage;

    public float DigDamage => _digDamage;
}