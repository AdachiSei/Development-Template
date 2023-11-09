using System;

namespace Template.Pause
{
    public class PauseData : IPauseable
    {
        public bool IsPausing { get; private set; } = false;

        private event Action OnPause = null;
        private event Action OnResume = null;

        public void Pause()
        {
            if (IsPausing = !IsPausing)
                OnPause?.Invoke();
            else
                OnResume?.Invoke();
        }

        public void RegisterPause(Action PauseMethod)
        {
            OnPause += PauseMethod;
        }

        public void RegisterResume(Action ResumeMethod)
        {
            OnResume += ResumeMethod;
        }

        public void ReleasePause(Action PauseMethod)
        {
            OnPause -= PauseMethod;
        }

        public void ReleaseResume(Action ResumeMethod)
        {
            OnResume -= ResumeMethod;
        }
    }
}
