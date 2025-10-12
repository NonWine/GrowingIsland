using System;
using Sirenix.OdinInspector;

[System.Serializable]
public class Stats
{
    public const float LabelWidht = 140f; 
    
    [ProgressBar(0, 100), ShowInInspector] [LabelWidth(LabelWidht)] public float MaxHealth { get; private set; }
    
    [ProgressBar(0, 100), ShowInInspector] [LabelWidth(LabelWidht)] public int MoveSpeed {get; private set; }
    [ProgressBar(0, 100), ShowInInspector] [LabelWidth(LabelWidht)] public int RotateSpeed {get; private set; }
    [ProgressBar(0, 100), ShowInInspector] [LabelWidth(LabelWidht)] public float RadiusDetection {get; private set; }
    [ProgressBar(0, 100), ShowInInspector] [LabelWidth(LabelWidht)] public float TimeBeetwenHit { get; private set; }
    [ProgressBar(0, 100), ShowInInspector] [LabelWidth(LabelWidht)] public float DamageHit { get; private set; }

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