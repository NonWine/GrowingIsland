using UnityEngine;
using Zenject;

[RequireComponent(typeof(EnvironmentPropObjectView))]
public class TreeDebugPreview : MonoBehaviour
{
    [Inject(Optional = true)] private TreePreviewController preview;

    [ContextMenu("Debug/Preview Hit")]
    private void PreviewHit()
    {
        if (!Application.isPlaying || preview == null)
        {
            return;
        }

        preview.PreviewHit();
    }

    [ContextMenu("Debug/Preview Final Fall")]
    private void PreviewFinalFall()
    {
        if (!Application.isPlaying || preview == null)
        {
            return;
        }

        preview.PreviewFinalFall();
    }

    [ContextMenu("Debug/Preview Full Final Hit")]
    private void PreviewFullFinalHit()
    {
        if (!Application.isPlaying || preview == null)
        {
            return;
        }

        preview.PreviewFullFinalHit();
    }

    [ContextMenu("Debug/Reset Preview")]
    private void ResetPreview()
    {
        if (!Application.isPlaying || preview == null)
        {
            return;
        }

        preview.ResetPreview();
    }
}