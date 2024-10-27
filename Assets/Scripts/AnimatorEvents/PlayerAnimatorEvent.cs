using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvent : MonoBehaviour
{
    public event Action OnAxeAttacked;

    public void OnAxeAttack()
    {
        OnAxeAttacked?.Invoke();
    }

}
