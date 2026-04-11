using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvent : MonoBehaviour
{
    public event Action OnFarming;

    public void OnFarmAttack()
    {
        OnFarming?.Invoke();
    }

    public void OnFootstep(AnimationEvent animationEvent)
    {
        // Receiver for imported walk/run clips that already send this event.
    }

    public void OnLand(AnimationEvent animationEvent)
    {
        // Receiver for imported landing clips that already send this event.
    }

}
