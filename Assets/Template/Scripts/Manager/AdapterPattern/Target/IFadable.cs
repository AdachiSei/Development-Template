
using Cysharp.Threading.Tasks;

namespace Template.Adapter
{
    /// <summary>
    /// フェードを呼び出す用のインターフェース
    /// </summary>
    public interface IFadable
    {
        #region Methods

        UniTask FadeIn();

        UniTask FadeOut();

        #endregion
    }
}
