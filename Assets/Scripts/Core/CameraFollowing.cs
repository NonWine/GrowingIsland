using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtual;
    [Inject] private PlayerContainer _playerContainer;
    private void Start()
    {
        SetFollow(_playerContainer.transform);

    }

    private void SetFollow(Transform target)
    {
        _cinemachineVirtual.Follow = target;
    }

}
