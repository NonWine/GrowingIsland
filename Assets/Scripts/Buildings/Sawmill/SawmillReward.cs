using System;
using Zenject;

public class SawmillReward : IInitializable, IDisposable
{
    private readonly IStorage _storage;
    private readonly CollectableManager _collectableManager;
    private readonly ISawmillView _view;

    public SawmillReward(IStorage storage, CollectableManager collectableManager, ISawmillView view)
    {
        _storage = storage;
        _collectableManager = collectableManager;
        _view = view;
    }

    public void Initialize() => _view.OnPlayerEntered += GiveAllToPlayer;
    public void Dispose() => _view.OnPlayerEntered -= GiveAllToPlayer;

    private void GiveAllToPlayer()
    {
        var amount = _storage.TakeAll();
        if (amount <= 0)
            return;

        _collectableManager.GetWallet(eCollectable.Wood)?.Add(amount);
    }
}
