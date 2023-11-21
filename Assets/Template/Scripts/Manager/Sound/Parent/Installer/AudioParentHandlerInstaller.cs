using Zenject;

/// <summary>
/// AudioParentHandlerのインストーラー
/// </summary>
public class AudioParentHandlerInstaller : Installer<AudioParentHandlerInstaller>
{
    public override void InstallBindings()
    {
        Container
        .Bind<IAudioParentHandler>()
        .To<AudioParentHandler>()
        .AsSingle();
    }
}
