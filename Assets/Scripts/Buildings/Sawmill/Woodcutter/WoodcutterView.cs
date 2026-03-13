using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class WoodcutterView : MonoBehaviour
{
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public AnimationEventsView AnimationEventsView { get; private set; }
    [field: SerializeField] public AudioSource AudioSource { get; private set; }

    [FormerlySerializedAs("_visualRoot")]
    [SerializeField] private Transform visualRoot;
    [FormerlySerializedAs("_heldLogAnchor")]
    [SerializeField] private Transform heldLogAnchor;
    [FormerlySerializedAs("_throwOrigin")]
    [SerializeField] private Transform throwOrigin;

    private Transform cachedHeldLogAnchor;

    public Transform VisualRoot => visualRoot != null
        ? visualRoot
        : Animator != null
            ? Animator.transform
            : transform;

    public Transform HeldLogAnchor => heldLogAnchor != null
        ? heldLogAnchor
        : cachedHeldLogAnchor != null
            ? cachedHeldLogAnchor
            : transform;

    public Transform ThrowOrigin => throwOrigin != null
        ? throwOrigin
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
        if (cachedHeldLogAnchor == null)
            cachedHeldLogAnchor = FindDeepChild(transform, "CATRHand") ?? FindDeepChild(transform, "CATLHand");
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
