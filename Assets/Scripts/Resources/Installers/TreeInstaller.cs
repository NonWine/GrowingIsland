using Zenject;

public class TreeInstaller : EnvironmentResourceInstaller
{
    protected override void InstallResourceBindings()
    {
        Container.Bind<ITreeHitReaction>().To<TreeHitReaction>().AsSingle();
        Container.Bind<ITreeFinalFallReaction>().To<TreeFinalFallReaction>().AsSingle();
        Container.Bind<ITreeDelayScheduler>().To<TreeTweenDelayScheduler>().AsSingle();
        Container.BindInterfacesAndSelfTo<TreeFinalFallSequence>().AsSingle();
       
        Container.Bind<TreeStumpController>().AsSingle();
        Container.Bind<TreeResourceDropExecutor>().AsSingle();
        Container.BindInterfacesAndSelfTo<TreePresentationController>().AsSingle();

        if (GetComponent<TreeDebugPreview>() != null)
        {
            Container.BindInterfacesAndSelfTo<TreePreviewController>().AsSingle();
        }
    }
}
