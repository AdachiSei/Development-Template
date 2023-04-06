using Template.Manager;
using UnityEngine;

namespace Template.Adapter
{
    /// <summary>
    /// フェードビューを使用するクライアント
    /// </summary>
    [RequireComponent(typeof(FadeViewClient))]
    [RequireComponent(typeof(DontDestroy))]
    public class FadeViewClient : MonoBehaviour
    {
        #region Member Variables

        private IFadable _fadable = null;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (TryGetComponent(out _fadable))
            {
                _fadable.FadeIn();
                SceneLoader.OnFadeIn += _fadable.FadeIn;
                SceneLoader.OnFadeOut += _fadable.FadeOut;
            }
        }

        #endregion
    }
}