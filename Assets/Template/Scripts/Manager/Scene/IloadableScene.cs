using Cysharp.Threading.Tasks;
using System;

namespace Template.Manager
{
    /// <summary>
    /// インターフェース
    /// </summary>
    public interface ILoadableScene
    {
        #region Properties

        string ActiveSceneName { get; }

        #endregion

        #region Methods

        UniTask LoadScene(string sceneName);

        void RegisterStartGame(Action startGame);

        void RegisterFadeIn(Func<UniTask> fadeInMethod);

        void RegisterFadeOut(Func<UniTask> fadeOutMethod);

        void ReleaseStartGame(Action startGame);

        void ReleaseFadeIn(Func<UniTask> fadeInMethod);

        void ReleaseFadeOut(Func<UniTask> fadeOutMethod);

        #endregion
    }
}
