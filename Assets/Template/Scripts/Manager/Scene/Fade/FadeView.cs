using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Template.Fade
{
    /// <summary>
    /// フェードを管理するView
    /// </summary>
    public class FadeView : IFadable
    {
        private Image _loadingImage = null;
        private Image _loadingPanel = null;
        private float _fadeTime = 1f;
        private Vector3 _turnDir = Vector3.forward * -360f;

        private const float LOADING_IMAGE_SPEED = 1f;
        private const int LOOP_VALUE = -1;
        private const float MAX_ALPFA = 1f;

        public FadeView(Image loadingImage, Image loadingPanel)
        {
            _loadingImage = loadingImage;
            _loadingPanel = loadingPanel;
            FadeIn().Forget();
        }

        public async UniTask FadeOut()
        {
            await _loadingPanel?.DOFade(MAX_ALPFA, _fadeTime);

            _loadingPanel?.DOKill();
            _loadingImage?.gameObject.SetActive(true);
            _loadingImage?
                .transform
                .DORotate(_turnDir, LOADING_IMAGE_SPEED, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(LOOP_VALUE);
        }

        public async UniTask FadeIn()
        {
            _loadingImage?.gameObject.SetActive(false);
            _loadingImage?.DOKill();

            await _loadingPanel?.DOFade(0f, _fadeTime);
        }
    }
}