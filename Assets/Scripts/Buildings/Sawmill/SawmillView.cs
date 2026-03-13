using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SawmillView : MonoBehaviour,
    ISawmillView,
    ISawmillCounterFeedbackView,
    ISawmillImpactFeedbackView,
    ISawmillPileVisualTarget,
    IPlayerEnterTriggable,
    IPlayerExitTriggable
{
    [SerializeField] private Transform _depositPoint;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private TMP_Text currentWoodText;
    [SerializeField] private GameObject fullStorageView;
    [SerializeField] private GameObject statusStorageView;
    [SerializeField] private bool _spawnWoodcutterOnStart = true;
    [SerializeField] private Transform _impactRoot;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private SawmillImpactFeedbackSettings _impactFeedback = new();
    [SerializeField] private SawmillCounterFeedbackSettings _counterFeedback = new();
    [SerializeField] private SawmillPileVisualSettings _pileVisuals = new();

    [SerializeField] private UnityEvent<int, int> _storageChangedEvent;
    [SerializeField] private UnityEvent<SawmillLevelSettings> _levelChangedEvent;
    [SerializeField] private UnityEvent _depositImpactEvent;
 
    public event Action OnPlayerEntered;
    public event Action OnPlayerExited;

    public Transform DepositPoint => _depositPoint != null ? _depositPoint : transform;
    public Transform SpawnPoint => _spawnPoint != null ? _spawnPoint : transform;
    public Vector3 WorldPosition => transform.position;
    public bool SpawnWoodcutterOnStart => _spawnWoodcutterOnStart;
    public TMP_Text CurrentWoodText => currentWoodText;
    public Transform ImpactRoot => _impactRoot != null ? _impactRoot : transform;
    public AudioSource AudioSource => _audioSource;
    public SawmillImpactFeedbackSettings ImpactFeedbackSettings => _impactFeedback;
    public SawmillCounterFeedbackSettings CounterFeedbackSettings => _counterFeedback;
    public SawmillPileVisualSettings PileVisualSettings => _pileVisuals;

    public void PlayerEnter() => OnPlayerEntered?.Invoke();
    public void PlayerExit() => OnPlayerExited?.Invoke();

    public void RenderStorage(int current, int capacity, bool isFull)
    {
        currentWoodText.text = current + "/" + capacity;
        fullStorageView.SetActive(isFull);
        statusStorageView.SetActive(!isFull);
    }

    public void NotifyStorageChanged(int current, int capacity)
        => _storageChangedEvent?.Invoke(current, capacity);

    public void NotifyLevelChanged(SawmillLevelSettings settings)
        => _levelChangedEvent?.Invoke(settings);

    public void NotifyDepositImpact()
        => _depositImpactEvent?.Invoke();
}
