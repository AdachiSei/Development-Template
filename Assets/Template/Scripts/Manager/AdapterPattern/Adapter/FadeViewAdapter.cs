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
    [RequireComponent(typeof(SceneLoaderClient))]
    [RequireComponent(typeof(DontDestroy))]
    public class FadeViewAdapter : FadeViewAdaptee, IFadable
    {
        #region Public Methods

        public async UniTask FadeInMethod()
        {
            await FadeIn();
        }

        public async UniTask FadeOutMethod()
        {
            await FadeOut();
        }

        #endregion
    }
}