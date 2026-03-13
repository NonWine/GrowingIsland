using System;
using Zenject;

public class SawmillPresenter : IInitializable, IDisposable
{
    private readonly ISawmillView _view;
    private readonly IStorage _storage;
    private readonly SawmillUpgrader _upgrader;
    private readonly IWoodcutterWorkplace _workplace;
    private readonly ISawmillStorageFeedback _storageFeedback;
    private readonly ISawmillImpactFeedback _impactFeedback;
    private readonly ISawmillPileVisualizer _pileVisualizer;

    private bool _hasRenderedStorage;
    private int _lastCurrent = -1;
    private int _lastCapacity = -1;

    public SawmillPresenter(
        ISawmillView view,
        IStorage storage,
        SawmillUpgrader upgrader,
        IWoodcutterWorkplace workplace,
        ISawmillStorageFeedback storageFeedback,
        ISawmillImpactFeedback impactFeedback,
        ISawmillPileVisualizer pileVisualizer)
    {
        _view = view;
        _storage = storage;
        _upgrader = upgrader;
        _workplace = workplace;
        _storageFeedback = storageFeedback;
        _impactFeedback = impactFeedback;
        _pileVisualizer = pileVisualizer;
    }

    public void Initialize()
    {
        _storage.OnStorageChanged += RenderStorage;
        _upgrader.LevelChanged += OnLevelChanged;
        _workplace.WoodDeposited += OnWoodDeposited;

        RenderStorage(_storage.Current, _storage.Capacity);
        _upgrader.NotifyInitial();
    }

    public void Dispose()
    {
        _storage.OnStorageChanged -= RenderStorage;
        _upgrader.LevelChanged -= OnLevelChanged;
        _workplace.WoodDeposited -= OnWoodDeposited;
    }


    private void OnLevelChanged(SawmillLevelSettings settings)
        => _view.NotifyLevelChanged(settings);

    private void OnWoodDeposited(float impactStrength)
    {
        _impactFeedback.Play(impactStrength);
        _pileVisualizer.PlayImpact(impactStrength);
        _view.NotifyDepositImpact();
    }

    private void RenderStorage(int current, int capacity)
    {
        bool stateChanged = !_hasRenderedStorage || current != _lastCurrent || capacity != _lastCapacity;
        if (_hasRenderedStorage && !stateChanged)
            return;

        bool animateFeedback = _hasRenderedStorage && stateChanged;
        bool isFull = capacity > 0 && current >= capacity;

        _lastCurrent = current;
        _lastCapacity = capacity;
        _hasRenderedStorage = true;

        _view.RenderStorage(current, capacity, isFull);
        _pileVisualizer.RenderStorage(current, capacity, animateFeedback);

        if (animateFeedback)
            _storageFeedback.PlayStorageChanged();

        _view.NotifyStorageChanged(current, capacity);
    }
}
