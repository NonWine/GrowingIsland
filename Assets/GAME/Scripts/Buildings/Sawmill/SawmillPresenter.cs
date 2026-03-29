using System;
using Zenject;

public class SawmillPresenter : IInitializable, IDisposable
{
    private readonly SawmillView view;
    private readonly IStorage storage;
    private readonly SawmillUpgrader upgrader;
    private readonly IWoodcutterWorkplace workplace;
    private readonly SawmillCounterAnimator storageFeedback;
    private readonly SawmillImpactFeedbackModule impactFeedback;
    private readonly SawmillPileVisualizer pileVisualizer;

    private bool hasRenderedStorage;
    private int lastCurrent = -1;
    private int lastCapacity = -1;

    public SawmillPresenter(
        SawmillView view,
        IStorage storage,
        SawmillUpgrader upgrader,
        IWoodcutterWorkplace workplace,
        SawmillCounterAnimator storageFeedback,
        SawmillImpactFeedbackModule impactFeedback,
        SawmillPileVisualizer pileVisualizer)
    {
        this.view = view;
        this.storage = storage;
        this.upgrader = upgrader;
        this.workplace = workplace;
        this.storageFeedback = storageFeedback;
        this.impactFeedback = impactFeedback;
        this.pileVisualizer = pileVisualizer;
    }

    public void Initialize()
    {
        storage.OnStorageChanged += RenderStorage;
        upgrader.LevelChanged += OnLevelChanged;
        workplace.WoodDeposited += OnWoodDeposited;

        RenderStorage(storage.Current, storage.Capacity);
        upgrader.NotifyInitial();
    }

    public void Dispose()
    {
        storage.OnStorageChanged -= RenderStorage;
        upgrader.LevelChanged -= OnLevelChanged;
        workplace.WoodDeposited -= OnWoodDeposited;
    }


    private void OnLevelChanged(SawmillLevelSettings settings)
        => view.NotifyLevelChanged(settings);

    private void OnWoodDeposited(float impactStrength)
    {
        impactFeedback.Play(impactStrength);
        pileVisualizer.PlayImpact(impactStrength);
        view.NotifyDepositImpact();
    }

    private void RenderStorage(int current, int capacity)
    {
        bool stateChanged = !hasRenderedStorage || current != lastCurrent || capacity != lastCapacity;
        if (hasRenderedStorage && !stateChanged)
            return;

        bool animateFeedback = hasRenderedStorage && stateChanged;
        bool isFull = capacity > 0 && current >= capacity;

        lastCurrent = current;
        lastCapacity = capacity;
        hasRenderedStorage = true;

        view.RenderStorage(current, capacity, isFull);
        pileVisualizer.RenderStorage(current, capacity, animateFeedback);

        if (animateFeedback)
            storageFeedback.PlayStorageChanged();

        view.NotifyStorageChanged(current, capacity);
    }
}
