using Template.Manager;
using Zenject;

/// <summary>
/// インストーラー
/// </summary>
public class ILoadableSceneInstaller : MonoInstaller<ILoadableSceneInstaller>
{
    public override void InstallBindings()
    {
        Container
            .Bind<ILoadableScene>()
            .To<SceneLoader>()
            .AsSingle();
    }
}