using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Template.Adapter
{
    /// <summary>
    /// フェードを管理するViewの機能を持つアダプティー
    /// </summary>
    public abstract class FadeViewAdaptee : MonoBehaviour
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

        #endregion

        #region Member Variables

        private Vector3 _rotDir = new(0f, 0f, -360);

        #endregion

        #region Constants

        private const float LOADING_IMAGE_SPEED = 1f;
        private const int LOOP_VALUE = -1;
        public const float MAX_ALPFA_VALUE = 1f;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            FadeIn();
        }

        #endregion

        #region Private Methods

        protected async UniTask<float> FadeIn()
        {
            _loadingImage?.gameObject.SetActive(false);
            _loadingImage?.DOKill();

            await _loadingPanel?.DOFade(0f, _fadeTime).AsyncWaitForCompletion();

            return _fadeTime;
        }

        protected async UniTask<float> FadeOut()
        {
            if (_loadingPanel != null)
                await _loadingPanel.DOFade(MAX_ALPFA_VALUE, _fadeTime).AsyncWaitForCompletion();

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