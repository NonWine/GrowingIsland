using System;
using DG.Tweening;
using Extensions;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private GameObject bridgePrefab; // Префаб мосту
    [SerializeField] private GameObject _obstakle;
    [SerializeField] private ConstructionZone _bridgeBuildZone;

    private bool hasBuilt;

    private void Awake()
    {
        hasBuilt = PlayerPrefsBool.GetBool(nameof(hasBuilt) + gameObject.name, false);
        bridgePrefab.SetActive(false);
        _bridgeBuildZone.OnComplete += BuildBridge;
        if(hasBuilt) BuildBridge();
    }

    private void OnDestroy()
    {
        _bridgeBuildZone.OnComplete -= BuildBridge;

    }

    public void BuildBridge()
    {
       // Instantiate(bridgePrefab, buildPoint.position, buildPoint.rotation);
       hasBuilt = true;
       PlayerPrefsBool.SetBool(nameof(hasBuilt) + gameObject.name, hasBuilt);
       bridgePrefab.SetActive(true);
       _obstakle.SetActive(false);
       bridgePrefab.transform.localScale = Vector3.zero;
       bridgePrefab.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
       _bridgeBuildZone.gameObject.SetActive(false);
       Debug.Log("Bridge built!");
    }
}