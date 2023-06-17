using System;

namespace Template.Manager
{
    /// <summary>
    /// インターフェース
    /// </summary>
    public interface ILoadableScene
    {
        #region Methods

        public event Action<string> LoadScene;

        #endregion
    }
}
