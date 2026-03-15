using UnityEngine;

public class TreeStumpController
{
    private readonly EnvironmentPropObjectView view;
    private readonly GameObject stumpInstance;

    public TreeStumpController(EnvironmentPropObjectView view)
    {
        this.view = view;
        if (view.StumpPrefab != null)
        {
            stumpInstance = Object.Instantiate(
                view.StumpPrefab,
                view.StumpAnchor.position,
                view.StumpAnchor.rotation,
                view.transform.parent);
            stumpInstance.SetActive(false);
        }
    }

    public void Show()
    {
        if (stumpInstance == null)
        {
            return;
        }

        stumpInstance.transform.SetPositionAndRotation(view.StumpAnchor.position, view.StumpAnchor.rotation);
        stumpInstance.SetActive(true);
    }

    public void Hide()
    {
        if (stumpInstance == null)
        {
            return;
        }

        stumpInstance.SetActive(false);
    }

    public void Dispose()
    {
        if (stumpInstance != null)
        {
            Object.Destroy(stumpInstance);
        }
    }
}
