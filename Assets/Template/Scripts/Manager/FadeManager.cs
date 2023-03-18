using Cysharp.Threading.Tasks;
using DG.Tweening;
using Template.Supporter;
using UnityEngine;
using UnityEngine.UI;

namespace Template.Manager
{
    /// <summary>
    /// フェードを管理するマネージャー
    /// </summary>
    [RequireComponent(typeof(SceneLoaderSupporter))]
    [RequireComponent(typeof(DontDestroy))]
    public class FadeManager : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        [Header("ローディング時に回転する絵")]
        private Image _loadingImage = null;

        [SerializeField]
        [Header("ローディング時に表示するパネル")]
        private Image _loadingPanel = null;

        [SerializeField]
        [Header("フェードするまでの時間")]
        private float _fadeTime = 1f;

        [SerializeField]
        [Header("回転する絵の回転速度")]
        private float _loadingImageSpeed = 1f;

        #endregion

        #region Member Variables

        private Vector3 _rotDir = new Vector3(0f, 0f, -360);
        private const int LOOP_VALUE = -1;

        #endregion

        #region Constants

        public const float MAX_ALPFA_VALUE = 1f;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            FadeIn();
        }

        #endregion

        #region Public Methods

        public async UniTask<float> FadeIn()
        {
            _loadingImage?.gameObject.SetActive(false);
            _loadingImage?.DOKill();

            await _loadingPanel?.DOFade(0f, _fadeTime).AsyncWaitForCompletion();

            return _fadeTime;
        }

        public async UniTask<float> FadeOut()
        {
            await _loadingPanel?.DOFade(MAX_ALPFA_VALUE, _fadeTime).AsyncWaitForCompletion();
            _loadingPanel?.DOKill();

            _loadingImage?.gameObject.SetActive(true);
            _loadingImage?
                .transform
                .DORotate(_rotDir, _loadingImageSpeed, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(LOOP_VALUE);

            return _fadeTime;
        }

        #endregion
    }
}