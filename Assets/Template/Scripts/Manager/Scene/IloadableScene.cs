using System;

namespace Template.Manager
{
    /// <summary>
    /// インターフェース
    /// </summary>
    public interface IloadableScene
    {
        #region Methods

        public event Action<string> LoadScene;

        #endregion
    }
}
