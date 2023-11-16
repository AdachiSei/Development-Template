using Zenject;

/// <summary>
/// VolumeDataのインストーラー
/// </summary>
public class VolumeDataInstaller : Installer<VolumeDataInstaller>
{
    public override void InstallBindings()
    {
        Container
        .Bind<IVolumeData>()
        .To<VolumeData>()
        .AsSingle();
    }
}
