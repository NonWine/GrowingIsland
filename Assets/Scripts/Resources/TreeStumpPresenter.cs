using UnityEngine;

public class TreeStumpPresenter
{
    private readonly TreeView view;
    private readonly GameObject stumpInstance;

    public TreeStumpPresenter(TreeView view)
    {
        this.view = view;
        stumpInstance = Object.Instantiate(
            view.StumpPrefab,
            view.StumpAnchor.position,
            view.StumpAnchor.rotation,
            view.transform.parent);
        stumpInstance.SetActive(false);
    }

    public void Show()
    {
        stumpInstance.transform.SetPositionAndRotation(view.StumpAnchor.position, view.StumpAnchor.rotation);
      //  stumpInstance.SetActive(true);
    }

    public void Hide()
    {
        stumpInstance.SetActive(false);
    }

    public void Dispose()
    {
        Object.Destroy(stumpInstance);
    }
}