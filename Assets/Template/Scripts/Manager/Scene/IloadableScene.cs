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

        #endregion
    }
}
