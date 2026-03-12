using UnityEngine;
using UnityEngine.AI;

public class WoodcutterView : MonoBehaviour
{
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public AnimationEventsView AnimationEventsView { get; private set; }
    [field: SerializeField] public AudioSource AudioSource { get; private set; }

    [SerializeField] private Transform _visualRoot;
    [SerializeField] private Transform _heldLogAnchor;
    [SerializeField] private Transform _throwOrigin;

    private Transform _cachedHeldLogAnchor;

    public Transform VisualRoot => _visualRoot != null
        ? _visualRoot
        : Animator != null
            ? Animator.transform
            : transform;

    public Transform HeldLogAnchor => _heldLogAnchor != null
        ? _heldLogAnchor
        : _cachedHeldLogAnchor != null
            ? _cachedHeldLogAnchor
            : transform;

    public Transform ThrowOrigin => _throwOrigin != null
        ? _throwOrigin
        : HeldLogAnchor != null
            ? HeldLogAnchor
            : VisualRoot;

    private void Awake()
    {
        CacheReferences();
    }

    private void OnValidate()
    {
        CacheReferences();
    }

    private void CacheReferences()
    {
        if (_cachedHeldLogAnchor == null)
            _cachedHeldLogAnchor = FindDeepChild(transform, "CATRHand") ?? FindDeepChild(transform, "CATLHand");
    }

    private static Transform FindDeepChild(Transform root, string childName)
    {
        if (root == null)
            return null;

        foreach (Transform child in root)
        {
            if (child.name == childName)
                return child;

            Transform result = FindDeepChild(child, childName);
            if (result != null)
                return result;
        }

        return null;
    }
}
