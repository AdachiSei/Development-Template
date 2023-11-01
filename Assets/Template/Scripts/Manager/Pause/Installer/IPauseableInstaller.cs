using Template.Manager;
using Zenject;

/// <summary>
/// スクリプト
/// </summary>
public class IPauseableInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<IPauseable>()
            .To<PauseData>()
            .AsSingle();
    }
}