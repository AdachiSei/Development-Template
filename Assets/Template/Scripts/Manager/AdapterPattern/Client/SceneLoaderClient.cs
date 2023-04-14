using Template.Manager;
using UnityEngine;

namespace Template.Adapter
{
    /// <summary>
    /// シーンをロード時にフェードをするクライアント
    /// </summary>
    [RequireComponent(typeof(FadeViewAdapter))]
    [RequireComponent(typeof(DontDestroy))]
    public class SceneLoaderClient : MonoBehaviour
    {
        #region Properties

        public SceneLoader SceneLoader { get; private set; } = new();

        #endregion

        #region Member Variables

        private IFadable _fadable = null;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (TryGetComponent(out _fadable))
            {
                _fadable.FadeInMethod();
                SceneLoader.OnFadeIn += _fadable.FadeInMethod;
                SceneLoader.OnFadeOut += _fadable.FadeOutMethod;
            }
        }

        #endregion
    }
}