using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyRequiredDataInstaller : MonoInstaller
{
    [SerializeField] private PatrolArea patrolArea;

    public override void InstallBindings()
    {
        Container.BindInstance(patrolArea).AsSingle();
      //  Debug.Log("bind patrol");
    }
}
