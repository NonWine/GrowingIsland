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

    [SerializeField] private ParticleSystem[] fallenLeaves; 
    private int currentPoof;
    private int currentAxeHit;
    private int currentMineHit;
    private int currentLeaves;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayPoof(Vector3 pos)
    {
        if (!HasItems(poofFx))
        {
            return;
        }

        poofFx[currentPoof].transform.position = pos;
        poofFx[currentPoof].Play();
        currentPoof++;
        if (currentPoof == poofFx.Length)
            currentPoof = 0;
    }

    public void PlayFallenLeaves(Vector3 pos)
    {
        if (!HasItems(fallenLeaves))
        {
            return;
        }

        var fx = fallenLeaves[currentLeaves];
        fx.transform.position = pos;
        fx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        fx.Clear(true);
        fx.Play(true);
        currentLeaves++;
        if (currentLeaves == fallenLeaves.Length)
            currentLeaves = 0;
    }

    
    public void PlayAxeHitFx(Vector3 pos)
    {
        if (!HasItems(lumberHitFx))
        {
            return;
        }

        lumberHitFx[currentAxeHit].transform.position = pos;
        lumberHitFx[currentAxeHit].Play();
        currentAxeHit++;
        if (currentAxeHit == lumberHitFx.Length)
            currentAxeHit = 0;
    }
    
    public void PlayMineHitFx(Vector3 pos)
    {
        if (!HasItems(mineHitFx))
        {
            return;
        }

        mineHitFx[currentMineHit].transform.position = pos;
        mineHitFx[currentMineHit].Play();
        currentMineHit++;
        if (currentMineHit == mineHitFx.Length)
            currentMineHit = 0;
    }

    private static bool HasItems(ParticleSystem[] pool) => pool != null && pool.Length > 0;
}
