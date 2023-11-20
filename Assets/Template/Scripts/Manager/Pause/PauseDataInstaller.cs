using Zenject;

namespace Template.Pause
{
    /// <summary>
    /// PauseDataのモノインストーラー
    /// </summary>
    public class PauseDataInstaller : MonoInstaller<PauseDataInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IPauseable>()
                .To<PauseData>()
                .AsSingle();
        }
    }
}