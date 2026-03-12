using System;
using System.Collections.Generic;
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
    [SerializeField] private Transform _impactRoot;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private SawmillImpactFeedbackSettings _impactFeedback = new();
    [SerializeField] private SawmillCounterFeedbackSettings _counterFeedback = new();
    [SerializeField] private SawmillPileVisualSettings _pileVisuals = new();

    [SerializeField] private UnityEvent<int, int> _storageChangedEvent;
    [SerializeField] private UnityEvent<SawmillLevelSettings> _levelChangedEvent;
    [SerializeField] private UnityEvent _depositImpactEvent;

    private readonly List<Transform> _pileStageRoots = new();
    private Sequence _impactSequence;
    private Sequence _uiSequence;
    private Color _counterBaseColor = Color.white;
    private bool _counterColorCached;
    private bool _storageInitialized;
    private int _lastCurrent;
    private int _lastCapacity;
    private int _builtPileCapacity = -1;

    public event Action OnPlayerEntered;
    public event Action OnPlayerExited;

    public Transform DepositPoint => _depositPoint != null ? _depositPoint : transform;
    public Transform SpawnPoint => _spawnPoint != null ? _spawnPoint : transform;
    public bool SpawnWoodcutterOnStart => _spawnWoodcutterOnStart;

    private void Awake()
    {
        CacheCounterColor();
        RebuildPileStages(_lastCapacity);
        UpdatePileVisuals(_lastCurrent, _lastCapacity, animateStageChange: false);
    }

    private void OnDestroy()
    {
        _impactSequence?.Kill();
        _uiSequence?.Kill();

        if (currentWoodText != null)
        {
            currentWoodText.DOKill();
            currentWoodText.rectTransform.DOKill();
        }

        ClearGeneratedPileStages();
    }

    public void PlayerEnter() => OnPlayerEntered?.Invoke();
    public void PlayerExit() => OnPlayerExited?.Invoke();

    public void ShowFullStorageView() => fullStorageView?.SetActive(true);
    public void HideFullStorageView() => fullStorageView?.SetActive(false);
    public void ShowStatusStorageView() => statusStorageView?.SetActive(true);
    public void HideStatusStorageView() => statusStorageView?.SetActive(false);

    public void OnStorageChanged(int current, int capacity)
    {
        bool animateFeedback = _storageInitialized;
        _storageInitialized = true;
        _lastCurrent = current;
        _lastCapacity = capacity;

        CacheCounterColor();
        UpdateCounterText(current, capacity);
        UpdateStatusViews(current, capacity);
        UpdatePileVisuals(current, capacity, animateFeedback);

        if (animateFeedback)
            PlayUiFeedback();

        _storageChangedEvent?.Invoke(current, capacity);
    }

    public void OnLevelChanged(SawmillLevelSettings settings)
    {
        if (settings != null)
            _lastCapacity = settings.StorageCapacity;

        RebuildPileStages(_lastCapacity);
        UpdatePileVisuals(_lastCurrent, _lastCapacity, animateStageChange: false);
        _levelChangedEvent?.Invoke(settings);
    }

    public void PlayReceiveAnimation(float impactStrength = 1f)
    {
        Transform impactRoot = _impactRoot != null ? _impactRoot : transform;
        float strength = Mathf.Max(0.2f, impactStrength);

        _impactSequence?.Kill();
        impactRoot.DOKill();

        _impactSequence = DOTween.Sequence();
        _impactSequence.Join(
            impactRoot.DOPunchScale(
                Vector3.one * (_impactFeedback.ScalePunch * strength),
                _impactFeedback.Duration,
                vibrato: 1,
                elasticity: 0f));

        if (_impactFeedback.PositionShake > 0f)
        {
            _impactSequence.Join(
                impactRoot.DOShakePosition(
                    _impactFeedback.Duration,
                    _impactFeedback.PositionShake * strength,
                    _impactFeedback.ShakeVibrato,
                    randomness: 90f,
                    snapping: false,
                    fadeOut: true));
        }

        if (_impactFeedback.RotationPunch > 0f)
        {
            _impactSequence.Join(
                impactRoot.DOPunchRotation(
                    new Vector3(0f, 0f, -_impactFeedback.RotationPunch * strength),
                    _impactFeedback.Duration,
                    vibrato: 1,
                    elasticity: 0f));
        }

        AnimatePileImpact(strength);
        PlayImpactVfx();
        PlayOneShot(_impactFeedback.ImpactClips, _impactFeedback.AudioVolume, _impactFeedback.AudioPitchRange);
        _depositImpactEvent?.Invoke();
    }

    private void UpdateCounterText(int current, int capacity)
    {
        if (currentWoodText == null)
            return;

        currentWoodText.text = current + "/" + capacity;
    }

    private void UpdateStatusViews(int current, int capacity)
    {
        bool isFull = capacity > 0 && current >= capacity;

        if (isFull)
        {
            HideStatusStorageView();
            ShowFullStorageView();
        }
        else
        {
            HideFullStorageView();
            ShowStatusStorageView();
        }
    }

    private void PlayUiFeedback()
    {
        if (currentWoodText == null)
            return;

        CacheCounterColor();
        currentWoodText.DOKill();
        currentWoodText.rectTransform.DOKill();
        currentWoodText.color = _counterBaseColor;

        _uiSequence?.Kill();
        _uiSequence = DOTween.Sequence();
        _uiSequence.Join(
            currentWoodText.rectTransform.DOPunchScale(
                Vector3.one * _counterFeedback.ScalePunch,
                _counterFeedback.ScalePunchDuration,
                vibrato: 1,
                elasticity: 0f));

        if (_counterFeedback.FlashDuration > 0f)
        {
            _uiSequence.Join(DOTween.To(
                    () => currentWoodText.color,
                    color => currentWoodText.color = color,
                    _counterFeedback.FlashColor,
                    _counterFeedback.FlashDuration * 0.5f)
                .SetLoops(2, LoopType.Yoyo));
        }
    }

    private void CacheCounterColor()
    {
        if (_counterColorCached || currentWoodText == null)
            return;

        _counterBaseColor = currentWoodText.color;
        _counterColorCached = true;
    }

    private void PlayImpactVfx()
    {
        if (_impactFeedback.ImpactVfxPrefab == null)
            return;

        ParticleSystem instance = Instantiate(
            _impactFeedback.ImpactVfxPrefab,
            DepositPoint.position,
            Quaternion.identity);

        Destroy(instance.gameObject, Mathf.Max(1f, instance.main.duration + instance.main.startLifetime.constantMax));
    }

    private void PlayOneShot(AudioClip[] clips, float volume, Vector2 pitchRange)
    {
        AudioClip clip = PickRandom(clips);
        if (clip == null)
            return;

        if (_audioSource != null)
        {
            _audioSource.pitch = UnityEngine.Random.Range(pitchRange.x, pitchRange.y);
            _audioSource.PlayOneShot(clip, volume);
            return;
        }

        AudioSource.PlayClipAtPoint(clip, DepositPoint.position, volume);
    }

    private static AudioClip PickRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return null;

        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }

    private void RebuildPileStages(int capacity)
    {
        if (!_pileVisuals.Enabled)
        {
            ClearGeneratedPileStages();
            return;
        }

        int stageCount = GetStageCount(capacity);
        if (_builtPileCapacity == capacity && _pileStageRoots.Count == stageCount)
            return;

        ClearGeneratedPileStages();
        _builtPileCapacity = capacity;

        GameObject pileTemplate = GetPileTemplate();
        if (pileTemplate == null)
            return;

        Transform pileRoot = _pileVisuals.PileRoot != null ? _pileVisuals.PileRoot : DepositPoint;
        if (pileRoot == null)
            return;

        for (int stageIndex = 0; stageIndex < stageCount; stageIndex++)
        {
            Transform stageRoot = new GameObject($"PileStage_{stageIndex + 1}").transform;
            stageRoot.SetParent(pileRoot, false);
            stageRoot.localPosition = _pileVisuals.BaseOffset
                                      + Vector3.up * (_pileVisuals.StageRise * stageIndex)
                                      + Vector3.forward * (_pileVisuals.StageDepth * stageIndex);
            stageRoot.localRotation = Quaternion.identity;
            stageRoot.localScale = Vector3.one;
            stageRoot.gameObject.SetActive(false);

            BuildPileStage(stageRoot, pileTemplate, stageIndex);
            _pileStageRoots.Add(stageRoot);
        }
    }

    private void BuildPileStage(Transform stageRoot, GameObject pileTemplate, int stageIndex)
    {
        int logsPerRow = Mathf.Max(1, _pileVisuals.LogsPerRow);

        for (int logIndex = 0; logIndex < Mathf.Max(1, _pileVisuals.LogsPerStage); logIndex++)
        {
            GameObject log = Instantiate(pileTemplate, stageRoot);
            PreparePileVisual(log);

            int row = logIndex / logsPerRow;
            int column = logIndex % logsPerRow;
            float centeredColumn = column - (logsPerRow - 1) * 0.5f;

            Transform logTransform = log.transform;
            logTransform.localPosition = new Vector3(
                centeredColumn * _pileVisuals.LogSpacing.x + UnityEngine.Random.Range(_pileVisuals.HorizontalJitter.x, _pileVisuals.HorizontalJitter.y),
                row * _pileVisuals.LogSpacing.y,
                UnityEngine.Random.Range(_pileVisuals.DepthJitter.x, _pileVisuals.DepthJitter.y));

            float baseYaw = _pileVisuals.LogBaseEuler.y + (stageIndex % 2 == 0 ? 0f : 90f);
            logTransform.localRotation = Quaternion.Euler(
                _pileVisuals.LogBaseEuler.x,
                baseYaw + UnityEngine.Random.Range(_pileVisuals.YawJitter.x, _pileVisuals.YawJitter.y),
                _pileVisuals.LogBaseEuler.z);
        }
    }

    private void PreparePileVisual(GameObject log)
    {
        log.SetActive(true);

        foreach (Collider collider in log.GetComponentsInChildren<Collider>(true))
            collider.enabled = false;

        foreach (Rigidbody rigidbody in log.GetComponentsInChildren<Rigidbody>(true))
            rigidbody.isKinematic = true;
    }

    private void ClearGeneratedPileStages()
    {
        for (int i = 0; i < _pileStageRoots.Count; i++)
        {
            if (_pileStageRoots[i] == null)
                continue;

            Destroy(_pileStageRoots[i].gameObject);
        }

        _pileStageRoots.Clear();
    }

    private GameObject GetPileTemplate()
    {
        if (_pileVisuals.LogPrefab != null)
            return _pileVisuals.LogPrefab;

        if (DepositPoint.childCount > 0)
            return DepositPoint.GetChild(0).gameObject;

        return null;
    }

    private void UpdatePileVisuals(int current, int capacity, bool animateStageChange)
    {
        RebuildPileStages(capacity);

        int visibleStages = GetVisibleStageCount(current, capacity);

        for (int i = 0; i < _pileStageRoots.Count; i++)
        {
            Transform stageRoot = _pileStageRoots[i];
            if (stageRoot == null)
                continue;

            bool shouldBeVisible = i < visibleStages;
            if (shouldBeVisible)
            {
                if (!stageRoot.gameObject.activeSelf)
                {
                    stageRoot.gameObject.SetActive(true);
                    stageRoot.localScale = animateStageChange ? Vector3.zero : Vector3.one;

                    if (animateStageChange)
                    {
                        stageRoot.DOScale(1f + _pileVisuals.StagePopScale, _pileVisuals.StagePopDuration)
                            .SetEase(Ease.OutBack)
                            .OnComplete(() =>
                                stageRoot.DOScale(1f, _pileVisuals.StageSettleDuration).SetEase(Ease.InOutSine));
                    }
                }
            }
            else
            {
                stageRoot.DOKill();
                stageRoot.localScale = Vector3.one;
                stageRoot.gameObject.SetActive(false);
            }
        }
    }

    private void AnimatePileImpact(float strength)
    {
        if (_pileStageRoots.Count == 0)
            return;

        int shakenStages = 0;
        for (int i = _pileStageRoots.Count - 1; i >= 0 && shakenStages < Mathf.Max(1, _impactFeedback.MaxPileShakeStages); i--)
        {
            Transform stageRoot = _pileStageRoots[i];
            if (stageRoot == null || !stageRoot.gameObject.activeSelf)
                continue;

            stageRoot.DOKill();
            stageRoot.DOPunchPosition(Vector3.up * (_impactFeedback.PileSettlePunch * strength), _impactFeedback.Duration * 0.85f, 1, 0f);
            stageRoot.DOPunchRotation(new Vector3(0f, _impactFeedback.PileRotationPunch * strength, 0f), _impactFeedback.Duration * 0.85f, 1, 0f);
            shakenStages++;
        }
    }

    private int GetStageCount(int capacity)
    {
        if (capacity <= 0)
            return 0;

        return Mathf.Max(1, Mathf.CeilToInt(capacity / (float)Mathf.Max(1, _pileVisuals.StageSize)));
    }

    private int GetVisibleStageCount(int current, int capacity)
    {
        if (current <= 0 || capacity <= 0)
            return 0;

        return Mathf.Min(_pileStageRoots.Count, Mathf.CeilToInt(current / (float)Mathf.Max(1, _pileVisuals.StageSize)));
    }
}

[Serializable]
public class SawmillImpactFeedbackSettings
{
    public float ScalePunch = 0.18f;
    public float Duration = 0.16f;
    public float PositionShake = 0.08f;
    public int ShakeVibrato = 10;
    public float RotationPunch = 4f;
    public float PileSettlePunch = 0.05f;
    public float PileRotationPunch = 2.5f;
    public int MaxPileShakeStages = 2;
    public ParticleSystem ImpactVfxPrefab;
    public AudioClip[] ImpactClips;
    public float AudioVolume = 1f;
    public Vector2 AudioPitchRange = new(0.95f, 1.05f);
}

[Serializable]
public class SawmillCounterFeedbackSettings
{
    public float ScalePunch = 0.16f;
    public float ScalePunchDuration = 0.16f;
    public Color FlashColor = new(1f, 0.94f, 0.74f, 1f);
    public float FlashDuration = 0.12f;
}

[Serializable]
public class SawmillPileVisualSettings
{
    public bool Enabled = true;
    public Transform PileRoot;
    public GameObject LogPrefab;
    public int StageSize = 5;
    public int LogsPerStage = 3;
    public int LogsPerRow = 3;
    public Vector3 BaseOffset = new(0f, 0f, 0f);
    public Vector3 LogSpacing = new(0.16f, 0.07f, 0.1f);
    public float StageRise = 0.07f;
    public float StageDepth = 0.12f;
    public Vector2 HorizontalJitter = new(-0.04f, 0.04f);
    public Vector2 DepthJitter = new(-0.04f, 0.04f);
    public Vector2 YawJitter = new(-12f, 12f);
    public Vector3 LogBaseEuler = new(0f, 90f, 0f);
    public float StagePopScale = 0.1f;
    public float StagePopDuration = 0.16f;
    public float StageSettleDuration = 0.1f;
}
