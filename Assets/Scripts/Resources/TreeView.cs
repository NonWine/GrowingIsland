using UnityEngine;

public class TreeView : MonoBehaviour
{
    [field: SerializeField] public Transform ReactionRoot { get; private set; }
    [field: SerializeField] public Transform CrownRoot { get; private set; }
    
    [field: SerializeField] public Transform[] LeavesPoints { get; private set; }
}
