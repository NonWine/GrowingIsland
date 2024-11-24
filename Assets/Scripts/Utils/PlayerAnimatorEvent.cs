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

}
