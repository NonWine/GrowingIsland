using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class EnvironmentPropObjectView : MonoBehaviour , IDamageable
{
    [SerializeField] private ResourceWorld resourceWorldAsset;
    [ShowInInspector] [Inject] private IAliveStateReader aliveStateReader;
    [SerializeField] private Transform reactionRoot;
    [SerializeField] private Transform fallRoot;
    [SerializeField] private Transform groundImpactPoint;
    [SerializeField] private Transform resourceDropOrigin;
    [SerializeField] private Transform[] leavesPoints;
    public event Action<float, Vector3> OnReceiveWorldDamage;

    public ResourceWorld ResourceWorldAsset => resourceWorldAsset;
    public eCollectable ResourceType => resourceWorldAsset.TypeWallet;
    public Transform ReactionRoot => reactionRoot;
    public Transform FallRoot => fallRoot;
    public Transform GroundImpactPoint => groundImpactPoint;
    public Transform ResourceDropOrigin => resourceDropOrigin;
    public Transform[] LeavesPoints => leavesPoints;
    public bool IsAlive => aliveStateReader.IsAlive;


    public void GetDamage(float damage, Vector3 sourceWorldPosition)
    {
        OnReceiveWorldDamage?.Invoke(damage,sourceWorldPosition);
    }
}
