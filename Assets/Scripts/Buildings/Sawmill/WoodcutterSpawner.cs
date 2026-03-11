using Zenject;

public class WoodcutterSpawner : IInitializable
{
    private readonly Sawmill _sawmill;
    private readonly SawmillView _view;
    private readonly WoodCutterFacade.Factory _factory;

    public WoodcutterSpawner(Sawmill sawmill, SawmillView view, WoodCutterFacade.Factory factory)
    {
        _sawmill = sawmill;
        _view = view;
        _factory = factory;
    }

    public void Initialize()
    {
        if (_view.SpawnWoodcutterOnStart)
            Spawn();
    }

    public void Spawn()
    {
        var woodcutter = _factory.Create(_sawmill);
        woodcutter.transform.position = _view.SpawnPoint.position;
    }
}
