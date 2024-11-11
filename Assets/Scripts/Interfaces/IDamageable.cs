using UnityEngine;

public interface IDamageable
{
    void GetDamage(float damage);

    bool isAlive { get; set; }
    
    Transform transform { get;  }
}