using System;

namespace Template.Pause
{
    /// <summary>
    /// インターフェース
    /// </summary>
    public interface IPauseable
    {
        bool IsPausing { get; }

        void Pause();

        void RegisterPause(Action PauseMethod);

        void RegisterResume(Action ResumeMethod);

        void ReleasePause(Action PauseMethod);

        void ReleaseResume(Action ResumeMethod);
    }
}
