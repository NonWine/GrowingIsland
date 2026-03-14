using UnityEngine;

[RequireComponent(typeof(Tree2))]
public class TreeDebugPreview : MonoBehaviour
{
    private Tree2 tree;

    private void Awake()
    {
        tree = GetComponent<Tree2>();
    }

    [ContextMenu("Debug/Preview Hit")]
    private void PreviewHit()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        tree.DebugPreviewHit();
    }

    [ContextMenu("Debug/Preview Final Fall")]
    private void PreviewFinalFall()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        tree.DebugPreviewFinalFall();
    }

    [ContextMenu("Debug/Preview Full Final Hit")]
    private void PreviewFullFinalHit()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        tree.DebugPreviewFullFinalHit();
    }

    [ContextMenu("Debug/Reset Preview")]
    private void ResetPreview()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        tree.DebugResetPreview();
    }
}