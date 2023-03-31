using Cysharp.Threading.Tasks;
using DG.Tweening;
using Template.Adapter;
using UnityEngine;
using UnityEngine.UI;

namespace Template.Adapter
{
    /// <summary>
    /// フェードを管理するViewの機能を持つアダプティーと
    /// フェードを呼び出す用のインターフェースを結合するアダプター
    /// </summary>
    [RequireComponent(typeof(FadeViewClient))]
    [RequireComponent(typeof(DontDestroy))]
    public class FadeViewAdapter : FadeViewAdaptee, IFadable
    {
        #region Public Methods

        public async UniTask<float> FadeInMethod()
        {
            _loadingImage?.gameObject.SetActive(false);
            _loadingImage?.DOKill();

            await _loadingPanel?.DOFade(0f, _fadeTime).AsyncWaitForCompletion();

            return _fadeTime;
        }

        public async UniTask<float> FadeOutMethod()
        {
            if (_loadingPanel != null)
                await _loadingPanel.DOFade(MAX_ALPFA, _fadeTime).AsyncWaitForCompletion();

            _loadingPanel?.DOKill();

            _loadingImage?.gameObject.SetActive(true);
            _loadingImage?
                .transform
                .DORotate(_rotDir, LOADING_IMAGE_SPEED, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(LOOP_VALUE);

            return _fadeTime;
        }

        #endregion
    }
}