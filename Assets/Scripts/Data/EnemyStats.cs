using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class EnemyStats : Stats
{
    [ProgressBar(0, 100), ShowInInspector] [LabelWidth(LabelWidht)] public float DistanceFromSpawn { get; private set; }
    [ProgressBar(0, 100), ShowInInspector] [LabelWidth(LabelWidht)] public float TargetDistance { get; private set; }

}