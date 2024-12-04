using System;
using DG.Tweening;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private GameObject bridgePrefab; // Префаб мосту
    [SerializeField] private GameObject _obstakle;
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
       _obstakle.SetActive(false);
       bridgePrefab.transform.localScale = Vector3.zero;
       bridgePrefab.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
       Debug.Log("Bridge built!");
    }
}