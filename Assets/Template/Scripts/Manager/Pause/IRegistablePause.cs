using System;

/// <summary>
/// インターフェース
/// </summary>
public interface IRegistablePause
{
    #region Methods

    void RegisterPause(Action PauseMethod);

    void RegisterResume(Action ResumeMethod);

    void ReleasePause(Action PauseMethod);

    void ReleaseResume(Action ResumeMethod);

    #endregion
}
