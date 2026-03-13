using Zenject;

public class WoodcutterSpawner : IInitializable
{
    private readonly IWoodcutterWorkplace workplace;
    private readonly ISawmillView view;
    private readonly WoodCutterFacade.Factory factory;

    public WoodcutterSpawner(IWoodcutterWorkplace workplace, ISawmillView view, WoodCutterFacade.Factory factory)
    {
        this.workplace = workplace;
        this.view = view;
        this.factory = factory;
    }

    public void Initialize()
    {
        if (view.SpawnWoodcutterOnStart)
            Spawn();
    }

    public void Spawn()
    {
        var woodcutter = factory.Create(workplace);
        woodcutter.transform.position = view.SpawnPoint.position;
    }
}
