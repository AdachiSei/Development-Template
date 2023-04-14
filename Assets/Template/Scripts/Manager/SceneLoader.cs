using Cysharp.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;

namespace Template.Manager
{
    /// <summary>
    /// シーンを読み込むスクリプト
    /// </summary>
    public class SceneLoader
    {
        #region Properties

        public string ActiveSceneName =>
            SceneManager.GetActiveScene().name;

        #endregion

        #region Member Variables

        private bool _isLoading = false;

        #endregion

        #region Events

        public event Action OnStartGame;
        public event Func<UniTask> OnFadeIn;
        public event Func<UniTask> OnFadeOut;

        #endregion

        #region Public Methods

        public async void LoadScene(string name)
        {
            if (_isLoading) return;
            _isLoading = true;

            if (OnFadeOut != null) await OnFadeOut();
            await SceneManager.LoadSceneAsync(name);
            await OnFadeIn();

            _isLoading = false;
            OnStartGame?.Invoke();
        }

        #endregion
    }
}