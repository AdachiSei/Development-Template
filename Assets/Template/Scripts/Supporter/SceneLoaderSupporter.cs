using Template.Manager;
using UnityEngine;

namespace Template.Supporter
{
    /// <summary>
    /// シーンローダーのサポーター
    /// </summary>
    [RequireComponent(typeof(SceneLoaderSupporter))]
    [RequireComponent(typeof(DontDestroy))]
    public class SceneLoaderSupporter : MonoBehaviour
    {
        #region Member Variables

        private FadeManager _fadeManager = null;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            TryGetComponent(out _fadeManager);
            SceneLoader.OnFadeIn += _fadeManager.FadeIn;
            SceneLoader.OnFadeOut += _fadeManager.FadeOut;
        }

        #endregion
    }
}