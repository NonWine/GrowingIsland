using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public static ParticlePool Instance;

    [SerializeField] private ParticleSystem[] _poofFx;
    [SerializeField] private ParticleSystem[] _lumberHitFx;

    private int _currentPoof;
    private int _currentAxeHit;

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

    
    public void PlayAxeHitFx(Vector3 pos)
    {
        _lumberHitFx[_currentAxeHit].transform.position = pos;
        _lumberHitFx[_currentAxeHit].Play();
        _currentAxeHit++;
        if (_currentAxeHit == _lumberHitFx.Length)
            _currentAxeHit = 0;
    }
}
