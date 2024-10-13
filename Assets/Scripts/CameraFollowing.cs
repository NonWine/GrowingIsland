using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtual;

    public void SetFollow(Transform target)
    {
        _cinemachineVirtual.Follow = target;
    }

}
