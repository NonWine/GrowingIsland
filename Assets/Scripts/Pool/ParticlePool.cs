using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public static ParticlePool Instance;

    [SerializeField] private ParticleSystem[] _poofFx;
    [SerializeField] private ParticleSystem[] _lumberHitFx;
    [SerializeField] private ParticleSystem[] _mineHitFx;

    private int _currentPoof;
    private int _currentAxeHit;
    private int _currentMineHit;

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
    
    public void PlayMineHitFx(Vector3 pos)
    {
        _mineHitFx[_currentMineHit].transform.position = pos;
        _mineHitFx[_currentMineHit].Play();
        _currentMineHit++;
        if (_currentMineHit == _mineHitFx.Length)
            _currentMineHit = 0;
    }
}
