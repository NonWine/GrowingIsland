using UnityEngine;

public static class EnvironmentResourceViewUtility
{
    public static void SetChildrenVisible(Transform root, bool isVisible)
    {
        for (int i = 0; i < root.childCount; i++)
        {
            root.GetChild(i).gameObject.SetActive(isVisible);
        }
    }

    public static Vector3 GetDefaultHitSource(Transform sourceTransform)
    {
        Vector3 defaultSource = sourceTransform.position - sourceTransform.forward;
        defaultSource.y = sourceTransform.position.y;
        return defaultSource;
    }
}
