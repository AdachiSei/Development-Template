using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Template.Scene
{
    /// <summary>
    /// シーンを読み込むスクリプト
    /// </summary>
    public class SceneLoader : ILoadableScene
    {
        public string ActiveSceneName =>
            SceneManager.GetActiveScene().name;

        private bool _isLoading = false;

        public async UniTask LoadScene(string sceneName)
        {
            if (_isLoading == true)
                return;

            _isLoading = true;
            await SceneManager.LoadSceneAsync(sceneName);
        }
    }
}