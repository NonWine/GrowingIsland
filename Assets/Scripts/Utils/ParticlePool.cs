using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ParticlePool : MonoBehaviour
{
    public static ParticlePool Instance;

    [FormerlySerializedAs("_poofFx")]
    [SerializeField] private ParticleSystem[] poofFx;
    [FormerlySerializedAs("_lumberHitFx")]
    [SerializeField] private ParticleSystem[] lumberHitFx;
    [FormerlySerializedAs("_mineHitFx")]
    [SerializeField] private ParticleSystem[] mineHitFx;

    private int currentPoof;
    private int currentAxeHit;
    private int currentMineHit;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayPoof(Vector3 pos)
    {
        poofFx[currentPoof].transform.position = pos;
        poofFx[currentPoof].Play();
        currentPoof++;
        if (currentPoof == poofFx.Length)
            currentPoof = 0;
    }

    
    public void PlayAxeHitFx(Vector3 pos)
    {
        lumberHitFx[currentAxeHit].transform.position = pos;
        lumberHitFx[currentAxeHit].Play();
        currentAxeHit++;
        if (currentAxeHit == lumberHitFx.Length)
            currentAxeHit = 0;
    }
    
    public void PlayMineHitFx(Vector3 pos)
    {
        mineHitFx[currentMineHit].transform.position = pos;
        mineHitFx[currentMineHit].Play();
        currentMineHit++;
        if (currentMineHit == mineHitFx.Length)
            currentMineHit = 0;
    }
}
