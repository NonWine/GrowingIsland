using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class EnemyStats : Stats
{
    [field: ProgressBar(0, 100), SerializeField] [LabelWidth(LabelWidht)] public float DistanceFromSpawn { get; private set; }
    [field: ProgressBar(0, 100), SerializeField] [LabelWidth(LabelWidht)] public float TargetDistance { get; private set; }

}