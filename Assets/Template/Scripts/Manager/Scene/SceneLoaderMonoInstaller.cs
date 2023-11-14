using Template.Scene;
using Zenject;

/// <summary>
/// インストーラー
/// </summary>
public class SceneLoaderMonoInstaller : MonoInstaller<SceneLoaderMonoInstaller>
{
    public override void InstallBindings()
    {
        Container
            .Bind(typeof(ILoadableScene),typeof(IRegestableLoadScene))
            .To<SceneLoader>()
            .AsSingle();
    }
}