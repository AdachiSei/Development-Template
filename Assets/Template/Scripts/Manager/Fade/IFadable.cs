using Cysharp.Threading.Tasks;

namespace Template.Fade
{
    /// <summary>
    /// フェードを呼び出す用のインターフェース
    /// </summary>
    public interface IFadable
    {
        UniTask FadeIn();
        UniTask FadeOut();
    }
}
