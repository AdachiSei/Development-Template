using UnityEngine;

namespace Template.Manager
{
    /// <summary>
    /// シーンローダーの関数を呼ぶためのクラス
    /// </summary>
    [RequireComponent(typeof(FadeViewAdapter))]
    public class SceneLoadCaller : MonoBehaviour
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
                SceneLoader.RegisterFadeIn(_fadable.FadeInMethod);
                SceneLoader.RegisterFadeOut(_fadable.FadeOutMethod);
            }
        }

        #endregion
    }
}