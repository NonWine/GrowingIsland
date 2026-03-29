using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class EnvironmentPropObjectView : MonoBehaviour, IWorldHitDamageable
{
    [FormerlySerializedAs("_resourceWorld")]
    [FormerlySerializedAs("resourceWorld")]
    [SerializeField] private ResourceWorld resourceWorldAsset;

    [Header("Tree Presentation")]
    [SerializeField] private Transform reactionRoot;
    [SerializeField] private Transform crownRoot;
    [SerializeField] private Transform fallRoot;
    [SerializeField] private Transform groundImpactPoint;
    [SerializeField] private Transform resourceDropOrigin;
    [SerializeField] private Transform stumpAnchor;
    [SerializeField] private GameObject stumpPrefab;
    [SerializeField] private Transform[] leavesPoints;

    [Inject(Optional = true)] private IEnvironmentResourceDamageService _damageWrapper;

    public ResourceWorld ResourceWorldAsset => resourceWorldAsset;
    public eCollectable ResourceType => resourceWorldAsset.TypeWallet;
    public bool IsAlive =>  _damageWrapper.IsAlive;
    public Transform ReactionRoot => reactionRoot;
    public Transform CrownRoot => crownRoot;
    public Transform FallRoot => fallRoot;
    public Transform GroundImpactPoint => groundImpactPoint;
    public Transform ResourceDropOrigin => resourceDropOrigin;
    public Transform StumpAnchor => stumpAnchor;
    public GameObject StumpPrefab => stumpPrefab;
    public Transform[] LeavesPoints => leavesPoints;

    public Vector3 GetDefaultHitSource()
    {
        Vector3 defaultSource = transform.position - transform.forward;
        defaultSource.y = transform.position.y;
        return defaultSource;
    }

    public void SetResourceVisualsVisible(bool isVisible)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(isVisible);
        }
    }

    bool IDamageable.isAlive
    {
        get => IsAlive;
        set { }
    }

    void IDamageable.GetDamage(float damage)
    {
        _damageWrapper.ApplyDamage(damage);
    }

    void IWorldHitDamageable.GetDamage(float damage, Vector3 sourceWorldPosition)
    {
        _damageWrapper.ApplyDamage(damage, sourceWorldPosition);
    }
}
