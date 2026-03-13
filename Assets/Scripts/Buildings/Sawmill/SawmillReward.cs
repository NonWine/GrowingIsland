using System;
using Zenject;

public class SawmillReward : IInitializable, IDisposable
{
    private readonly IStorage storage;
    private readonly CollectableManager collectableManager;
    private readonly SawmillView view;

    public SawmillReward(IStorage storage, CollectableManager collectableManager, SawmillView view)
    {
        this.storage = storage;
        this.collectableManager = collectableManager;
        this.view = view;
    }

    public void Initialize() => view.OnPlayerEntered += GiveAllToPlayer;
    public void Dispose() => view.OnPlayerEntered -= GiveAllToPlayer;

    private void GiveAllToPlayer()
    {
        var amount = storage.TakeAll();
        if (amount <= 0)
            return;

        collectableManager.GetWallet(eCollectable.Wood)?.Add(amount);
    }
}
