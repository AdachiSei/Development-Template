using System;

namespace Template.Manager
{
    /// <summary>
    /// インターフェース
    /// </summary>
    public interface IPauseable
    {
        #region Properties

        bool IsPausing { get; }

        #endregion

        #region Methods

        void Pause();

        void RegisterPause(Action PauseMethod);

        void RegisterResume(Action ResumeMethod);

        void ReleasePause(Action PauseMethod);

        void ReleaseResume(Action ResumeMethod);

        #endregion
    }
}
