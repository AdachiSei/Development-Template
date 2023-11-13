using Cysharp.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;

namespace Template.Scene
{
    /// <summary>
    /// シーンを読み込むスクリプト
    /// </summary>
    public class SceneLoader : ILoadableScene, IRegestableLoadScene
    {
        public string ActiveSceneName =>
            SceneManager.GetActiveScene().name;

        private bool _isLoading = false;

        private event Action OnStartGame = null;
        private event Func<UniTask> OnFadeIn = null;
        private event Func<UniTask> OnFadeOut = null;

        public async UniTask LoadScene(string sceneName)
        {
            if (_isLoading == true)
                return;

            _isLoading = true;

            if(OnFadeOut != null)
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

    }
}