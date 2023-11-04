using Template.Manager;
using Zenject;

/// <summary>
/// スクリプト
/// </summary>
public class IPauseableInstaller : MonoInstaller<IPauseableInstaller>
{
    public override void InstallBindings()
    {
        Container
            .Bind<IPauseable>()
            .To<PauseData>()
            .AsSingle();
    }
}