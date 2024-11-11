using UnityEngine;

public interface IDamageableHandler
{
    void HandDamage(float damageSend, out bool isDetected);
    
    void HandDamage(float damageSend, out bool isDetected, out Transform target);

}