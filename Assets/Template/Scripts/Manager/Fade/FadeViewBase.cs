using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Template.Manager
{
    /// <summary>
    /// フェードを管理するViewの機能を持つ基底クラス
    /// </summary>
    public abstract class FadeViewBase : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        [Header("ローディング時に回転する絵")]
        protected Image _loadingImage = null;

        [SerializeField]
        [Header("ローディング時に表示するパネル")]
        protected Image _loadingPanel = null;

        [SerializeField]
        [Header("フェードするまでの時間")]
        protected float _fadeTime = 1f;

        #endregion

        #region Member Variables

        protected Vector3 _rotDir = new(0f, 0f, -360f);

        #endregion

        #region Constants

        protected const float LOADING_IMAGE_SPEED = 1f;
        protected const int LOOP_VALUE = -1;
        protected const float MAX_ALPFA = 1f;

        #endregion

        #region Private Methods

        protected async UniTask FadeIn()
        {
            _loadingImage?.gameObject.SetActive(false);
            _loadingImage?.DOKill();
            _loadingPanel?.DOFade(0f, _fadeTime);
            await UniTask.Delay(TimeSpan.FromSeconds(_fadeTime));
        }

        protected async UniTask FadeOut()
        {
            if (_loadingPanel != null)
            {
                await _loadingPanel.DOFade(MAX_ALPFA, _fadeTime);
            }

            _loadingPanel?.DOKill();

            _loadingImage?.gameObject.SetActive(true);
            _loadingImage?
                .transform
                .DORotate(_rotDir, LOADING_IMAGE_SPEED, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(LOOP_VALUE);
        }

        #endregion
    }
}