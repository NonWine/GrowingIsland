using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;
using UnityEngine.Serialization;

public class CameraFollowing : MonoBehaviour
{
    [FormerlySerializedAs("_cinemachineVirtual")]
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtual;

    [Inject] private Player playerContainer;
    
    private void Start()
    {
        SetFollow(playerContainer.transform);

    }

    private void SetFollow(Transform target)
    {
        cinemachineVirtual.Follow = target;
    }
 
}
