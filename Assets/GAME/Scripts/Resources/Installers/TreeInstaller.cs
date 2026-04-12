using Zenject;

public class TreeInstaller : EnvironmentResourceInstaller
{
    protected override void InstallResourceBindings()
    {
        Container.Bind<TreeLeavesBurster>().AsSingle();
        Container.BindInterfacesAndSelfTo<TreeHitReaction>().AsSingle();
        Container.BindInterfacesAndSelfTo<TreeFinalFallReaction>().AsSingle();
       
        Container.BindInterfacesAndSelfTo<TreePresentationController>().AsSingle();

        if (GetComponent<TreeDebugPreview>() != null)
        {
            Container.BindInterfacesAndSelfTo<TreePreviewController>().AsSingle();
        }
    }

    protected override void InstallDropSpawner()
    {
        Container.BindInterfacesAndSelfTo<TreeResourceDropExecutor>().AsSingle();
    }
}
