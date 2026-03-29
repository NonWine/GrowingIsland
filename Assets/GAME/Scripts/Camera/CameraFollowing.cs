using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using UnityEngine.Serialization;

public class CameraFollowing : MonoBehaviour
{
    [FormerlySerializedAs("_cinemachineVirtual")]
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtual;
    [SerializeField] private Camera mainCamera;
    [Inject] private Player playerContainer;
    
    private void Start()
    {
        SetFollow(playerContainer.transform);

    }

    private void SetFollow(Transform target)
    {
        cinemachineVirtual.Follow = playerContainer.PlayerContainer.CameraRoot;
        playerContainer.PlayerContainer.GetComponentInChildren<PlayerInput>().camera = mainCamera;
    }
 
}
