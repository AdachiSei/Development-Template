using Cysharp.Threading.Tasks;

namespace Template.Scene
{
    /// <summary>
    /// インターフェース
    /// </summary>
    public interface ILoadableScene
    {
        string ActiveSceneName { get; }

        UniTask LoadScene(string sceneName);
    }
}
