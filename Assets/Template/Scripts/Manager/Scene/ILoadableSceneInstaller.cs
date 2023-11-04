using Template.Manager;
using Zenject;

/// <summary>
/// スクリプト
/// </summary>
public class ILoadableSceneInstaller : MonoInstaller<IPauseableInstaller>
{
    public override void InstallBindings()
    {
        Container
            .Bind<ILoadableScene>()
            .To<SceneLoader>()
            .AsSingle();
    }
}