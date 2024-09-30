using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraFollowing : MonoBehaviour
{
    private Transform _player;
    private Camera _camera;
    private Vector3 _startCamPos;
    
    private void Start()
    {
        _camera = Camera.main;
        _startCamPos = _camera.transform.position;
    }

    public void Init(Transform transform)
    {
        _player = transform;
    }

    private void LateUpdate()
    {
        FollowToPlayer();
    }

    private void FollowToPlayer()
    {
        if (transform.position != _player.position)
            _camera.transform.position = Vector3.MoveTowards(  _camera.transform.position,_player.position + _startCamPos , 3f);
    }

}
