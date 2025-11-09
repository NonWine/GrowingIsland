using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class EnemyStats : Stats
{
    [field: ProgressBar(0, 15), SerializeField] [LabelWidth(LabelWidht)] public float LeashRadius { get; private set; }
    [field: ProgressBar(0, 5), SerializeField] [LabelWidth(LabelWidht)] public float AttackRadius { get; private set; }

}