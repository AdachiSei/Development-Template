using System;

/// <summary>
/// インターフェース
/// </summary>
public interface IRegistablePauseAndResume
{
    #region Methods

    public void RegisterPause(Action PauseMethod);

    public void RegisterResume(Action ResumeMethod);

    public void ReleasePause(Action PauseMethod);

    public void ReleaseResume(Action ResumeMethod);

    #endregion
}
