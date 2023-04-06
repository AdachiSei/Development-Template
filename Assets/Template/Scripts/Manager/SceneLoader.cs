using Cysharp.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;

namespace Template.Manager
{
    /// <summary>
    /// シーンを読み込むスクリプト
    /// </summary>
    public static class SceneLoader
    {
        #region Properties

        public static string ActiveSceneName =>
            SceneManager.GetActiveScene().name;

        #endregion

        #region Member Variables

        private static bool _isLoading = false;

        #endregion

        #region Events

        public static event Action OnStartGame;
        public static event Func<UniTask> OnFadeIn;
        public static event Func<UniTask> OnFadeOut;

        #endregion

        #region Public Methods

        public async static void LoadScene(string name)
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