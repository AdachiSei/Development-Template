using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Template.Manager
{
    /// <summary>
    /// フェードを管理するViewの機能を持つアダプティーと
    /// フェードを呼び出す用のインターフェースを結合するアダプター
    /// </summary>
    [RequireComponent(typeof(SceneLoaderClient))]
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