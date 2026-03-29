using System;
using DG.Tweening;
using Extensions;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private GameObject bridgePrefab; // Префаб мосту
    [FormerlySerializedAs("_obstakle")]
    [SerializeField] private GameObject obstakle;
    [FormerlySerializedAs("_bridgeBuildZone")]
    [SerializeField] private ConstructionZone bridgeBuildZone;

    private bool hasBuilt;

    private void Awake()
    {
        hasBuilt = PlayerPrefsBool.GetBool(nameof(hasBuilt) + gameObject.name, false);
        bridgePrefab.SetActive(false);
        bridgeBuildZone.OnComplete += BuildBridge;
        if(hasBuilt) BuildBridge();
    }

    private void OnDestroy()
    {
        bridgeBuildZone.OnComplete -= BuildBridge;

    }

    public void BuildBridge()
    {
       // Instantiate(bridgePrefab, buildPoint.position, buildPoint.rotation);
       hasBuilt = true;
       PlayerPrefsBool.SetBool(nameof(hasBuilt) + gameObject.name, hasBuilt);
       bridgePrefab.SetActive(true);
       obstakle.SetActive(false);
       bridgePrefab.transform.localScale = Vector3.zero;
       bridgePrefab.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
       bridgeBuildZone.gameObject.SetActive(false);
       Debug.Log("Bridge built!");
    }
}
