using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Template.Manager
{
    /// <summary>
    /// シーンを読み込むスクリプト
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        #region Properties

        public string CurrentScene => SceneManager.GetActiveScene().name;

        #endregion

        #region Inspector Variables

        [SerializeField]
        [Header("ローディング時に回転する絵")]
        Image _loadingImage = null;

        [SerializeField]
        [Header("ローディング時に表示するパネル")]
        Image _loadingPanel = null;

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

        #region Unity Method

        private void Awake()
        {
            FadeIn();
            _loadingImage?.gameObject.SetActive(false);
        }

        #endregion

        #region Public Method

        async public void LoadScene(string name)
        {
            await FadeOut();

            var tween =
                _loadingImage?
                    .transform
                    .DORotate(_rotDir, _loadingImageSpeed,
                                RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
                    .SetLoops(LOOP_VALUE);

            await SceneManager.LoadSceneAsync(name);

            tween.Kill();
        }

        #endregion

        #region Private Methods

        private void FadeIn()
        {
            _loadingPanel?.DOFade(0f, _fadeTime);
        }

        async private UniTask FadeOut()
        {
            _loadingPanel.DOFade(MAX_ALPFA_VALUE, _fadeTime);

            await UniTask.Delay(TimeSpan.FromSeconds(_fadeTime));
        }

        #endregion
    }
}