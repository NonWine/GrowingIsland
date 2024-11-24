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