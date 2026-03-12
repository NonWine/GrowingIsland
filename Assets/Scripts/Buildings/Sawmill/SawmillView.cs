using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SawmillView : MonoBehaviour, IPlayerEnterTriggable, IPlayerExitTriggable
{
    [SerializeField] private Transform _depositPoint;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private TMP_Text currentWoodText;
    [SerializeField] private GameObject fullStorageView;
    [SerializeField] private GameObject statusStorageView;
    [SerializeField] private bool _spawnWoodcutterOnStart = true;
    [SerializeField] private float _receivePunchScale = 0.22f;
    [SerializeField] private float _receivePunchDuration = 0.14f;

    [SerializeField] private UnityEvent<int, int> _storageChangedEvent;
    [SerializeField] private UnityEvent<SawmillLevelSettings> _levelChangedEvent;

    public event Action OnPlayerEntered;
    public event Action OnPlayerExited;

    public Transform DepositPoint => _depositPoint != null ? _depositPoint : transform;
    public Transform SpawnPoint => _spawnPoint != null ? _spawnPoint : transform;
    public bool SpawnWoodcutterOnStart => _spawnWoodcutterOnStart;

    public void PlayerEnter() => OnPlayerEntered?.Invoke();
    public void PlayerExit() => OnPlayerExited?.Invoke();

    public void ShowFullStorageView() => fullStorageView.SetActive(true);
    public void HideFullStorageView() => fullStorageView.SetActive(false);
    public void ShowStatusStorageView() => statusStorageView.SetActive(true);
    public void HideStatusStorageView() => statusStorageView.SetActive(false);

    public void OnStorageChanged(int current, int capacity)
    {
        currentWoodText.text = current.ToString() + "/" + capacity.ToString();
        
        if (current >= capacity)
        {
            HideStatusStorageView();
            ShowFullStorageView();
        }
        else
        {
            HideFullStorageView();
            ShowStatusStorageView();
        }
        
        _storageChangedEvent?.Invoke(current, capacity);
    }
    public void OnLevelChanged(SawmillLevelSettings settings) => _levelChangedEvent?.Invoke(settings);

    public void PlayReceiveAnimation()
    {
        transform.DOPunchScale(Vector3.one * _receivePunchScale, _receivePunchDuration, vibrato: 1, elasticity: 0f);
    }
    
}
