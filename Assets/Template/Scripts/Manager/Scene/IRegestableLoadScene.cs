using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// インターフェース
/// </summary>
public interface IRegestableLoadScene
{
    void RegisterStartGame(Action startGame);
    void RegisterFadeIn(Func<UniTask> fadeInMethod);
    void RegisterFadeOut(Func<UniTask> fadeOutMethod);
    void ReleaseStartGame(Action startGame);
    void ReleaseFadeIn(Func<UniTask> fadeInMethod);
    void ReleaseFadeOut(Func<UniTask> fadeOutMethod);
}
