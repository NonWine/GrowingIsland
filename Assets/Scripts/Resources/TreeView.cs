using UnityEngine;

public class TreeView : MonoBehaviour
{
    [field: SerializeField] public Transform ReactionRoot { get; private set; }
    [field: SerializeField] public Transform CrownRoot { get; private set; }
    [field: SerializeField] public Transform FallRoot { get; private set; }
    [field: SerializeField] public Transform GroundImpactPoint { get; private set; }
    [field: SerializeField] public Transform StumpAnchor { get; private set; }
    [field: SerializeField] public GameObject StumpPrefab { get; private set; }
    [field: SerializeField] public Transform[] LeavesPoints { get; private set; }
}
