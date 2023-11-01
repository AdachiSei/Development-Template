using Cysharp.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;

namespace Template.Manager
{
    /// <summary>
    /// シーンを読み込むスクリプト
    /// </summary>
    public class SceneLoader : ILoadableScene
    {
        #region Properties

        public string ActiveSceneName =>
            SceneManager.GetActiveScene().name;

        #endregion

        #region Member Variables

        private bool _isLoading = false;

        #endregion

        #region Events

        private event Action OnStartGame;
        private event Func<UniTask> OnFadeIn;
        private event Func<UniTask> OnFadeOut;

        #endregion

        #region Public Methods

        public async UniTask LoadScene(string sceneName)
        {
            if (_isLoading)
                return;

            _isLoading = true;

            if (OnFadeOut != null)
                await OnFadeOut();

            await SceneManager.LoadSceneAsync(sceneName);

            if (OnFadeIn != null)
                await OnFadeIn();

            _isLoading = false;
            OnStartGame?.Invoke();
        }

        public void RegisterStartGame(Action startGame)
        {
            OnStartGame += startGame;
        }

        public void RegisterFadeIn(Func<UniTask> fadeInMethod)
        {
            OnFadeIn += fadeInMethod;
        }

        public void RegisterFadeOut(Func<UniTask> fadeOutMethod)
        {
            OnFadeOut += fadeOutMethod;
        }

        public void ReleaseStartGame(Action startGame)
        {
            OnStartGame -= startGame;
        }

        public void ReleaseFadeIn(Func<UniTask> fadeInMethod)
        {
            OnFadeIn -= fadeInMethod;
        }

        public void ReleaseFadeOut(Func<UniTask> fadeOutMethod)
        {
            OnFadeOut -= fadeOutMethod;
        }

        #endregion
    }
}