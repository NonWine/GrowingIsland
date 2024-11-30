using System;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private GameObject bridgePrefab; // Префаб мосту
    [SerializeField] private ConstructionZone _bridgeBuildZone;

    private void Awake()
    {
        bridgePrefab.SetActive(false);
        _bridgeBuildZone.OnComplete += BuildBridge;
    }

    private void OnDestroy()
    {
        _bridgeBuildZone.OnComplete -= BuildBridge;

    }

    public void BuildBridge()
    {
       // Instantiate(bridgePrefab, buildPoint.position, buildPoint.rotation);
       bridgePrefab.SetActive(true);
       Debug.Log("Bridge built!");
    }
}