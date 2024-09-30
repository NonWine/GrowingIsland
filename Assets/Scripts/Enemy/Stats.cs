using System;
using UnityEngine;

[System.Serializable]
public class Stats
{
    [SerializeField] private int _health;
    [SerializeField] private int _moveSpeed;
    [SerializeField] private int _roateSpeed;
    private int _currentHealth;
    private bool _isDeath;

    public event Action<int> OnHealthChange;
    
    public int MoveSpeed => _moveSpeed;
    
    public int RotateSpeed => _moveSpeed;

    public int MaxHealth => _health;
    
    public int CurrentHealth
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
    [SerializeField] private int _damage;
    [SerializeField] private float _coolDown;
    [SerializeField] private int _maxEnemyDetection;
    [SerializeField] private float _radiusDetection;
    public int Damage => _damage;

    public float CoolDown => _coolDown;

    public int MaxEnemyDetection => _maxEnemyDetection;

    public float RadiusDetection => _radiusDetection;
}