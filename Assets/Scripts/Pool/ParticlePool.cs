using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public static ParticlePool Instance;

    [SerializeField] private ParticleSystem[] _poofFx;
    private int _currentPoof;
    
    private void Awake()
    {
        Instance = this;
    }

    public void PlayPoof(Vector3 pos)
    {
        _poofFx[_currentPoof].transform.position = pos;
        _poofFx[_currentPoof].Play();
        _currentPoof++;
        if (_currentPoof == _poofFx.Length)
            _currentPoof = 0;
    }


}
