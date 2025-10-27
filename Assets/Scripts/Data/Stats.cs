using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[System.Serializable]
public class Stats
{
    public const float LabelWidht = 140f; 
    
    [field: ProgressBar(0, 100), SerializeField] [LabelWidth(LabelWidht)] public float MaxHealth { get; private set; }
    
    [field: ProgressBar(0, 100), SerializeField] [LabelWidth(LabelWidht)] public int MoveSpeed {get; private set; }
    [field: ProgressBar(0, 100), SerializeField] [LabelWidth(LabelWidht)] public int RotateSpeed {get; private set; }
    [field: ProgressBar(0, 100), SerializeField] [LabelWidth(LabelWidht)] public float RadiusDetection {get; private set; }
    [field: ProgressBar(0, 100), SerializeField] [LabelWidth(LabelWidht)] public float TimeBeetwenHit { get; private set; }
    [field: ProgressBar(0, 100), SerializeField] [LabelWidth(LabelWidht)] public float DamageHit { get; private set; }

    private float _currentHealth;
    
    public event Action<float> OnHealthChange;
    

    
    public float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            OnHealthChange?.Invoke(_currentHealth);
        }
    }
    

}