using Cysharp.Threading.Tasks;

namespace Template.Manager
{
    /// <summary>
    /// フェードを呼び出す用のインターフェース
    /// </summary>
    public interface IFadable
    {
        #region Methods

        UniTask FadeInMethod();

        UniTask FadeOutMethod();

        #endregion
    }
}
