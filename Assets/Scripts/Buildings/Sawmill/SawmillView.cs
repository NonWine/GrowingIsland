using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class SawmillView : MonoBehaviour,
    ISawmillView,
    ISawmillCounterFeedbackView,
    ISawmillImpactFeedbackView,
    ISawmillPileVisualTarget,
    IPlayerEnterTriggable,
    IPlayerExitTriggable
{
    [FormerlySerializedAs("_depositPoint")]
    [SerializeField] private Transform depositPoint;
    [FormerlySerializedAs("_spawnPoint")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private TMP_Text currentWoodText;
    [SerializeField] private GameObject fullStorageView;
    [SerializeField] private GameObject statusStorageView;
    [FormerlySerializedAs("_spawnWoodcutterOnStart")]
    [SerializeField] private bool spawnWoodcutterOnStart = true;
    [FormerlySerializedAs("_impactRoot")]
    [SerializeField] private Transform impactRoot;
    [FormerlySerializedAs("_pileRoot")]
    [SerializeField] private Transform pileRoot;
    [FormerlySerializedAs("_audioSource")]
    [SerializeField] private AudioSource audioSource;

    [FormerlySerializedAs("_storageChangedEvent")]
    [SerializeField] private UnityEvent<int, int> storageChangedEvent;
    [FormerlySerializedAs("_levelChangedEvent")]
    [SerializeField] private UnityEvent<SawmillLevelSettings> levelChangedEvent;
    [FormerlySerializedAs("_depositImpactEvent")]
    [SerializeField] private UnityEvent depositImpactEvent;
 
    public event Action OnPlayerEntered;
    public event Action OnPlayerExited;

    public Transform DepositPoint => depositPoint != null ? depositPoint : transform;
    public Transform SpawnPoint => spawnPoint != null ? spawnPoint : transform;
    public Vector3 WorldPosition => transform.position;
    public bool SpawnWoodcutterOnStart => spawnWoodcutterOnStart;
    public TMP_Text CurrentWoodText => currentWoodText;
    public Transform ImpactRoot => impactRoot != null ? impactRoot : transform;
    public Transform PileRoot => pileRoot != null ? pileRoot : DepositPoint;
    public AudioSource AudioSource => audioSource;

    public void PlayerEnter() => OnPlayerEntered?.Invoke();
    public void PlayerExit() => OnPlayerExited?.Invoke();

    public void RenderStorage(int current, int capacity, bool isFull)
    {
        currentWoodText.text = current + "/" + capacity;
        fullStorageView.SetActive(isFull);
        statusStorageView.SetActive(!isFull);
    }

    public void NotifyStorageChanged(int current, int capacity)
        => storageChangedEvent?.Invoke(current, capacity);

    public void NotifyLevelChanged(SawmillLevelSettings settings)
        => levelChangedEvent?.Invoke(settings);

    public void NotifyDepositImpact()
        => depositImpactEvent?.Invoke();
}
