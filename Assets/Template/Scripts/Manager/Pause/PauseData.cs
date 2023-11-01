using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Manager
{
    public class PauseData : IPauseable, IRegistablePause
    {
        #region Properties

        public bool IsPausing { get; private set; } = false;

        #endregion

        #region Events

        private event Action OnPause = null;
        private event Action OnResume = null;

        #endregion

        #region Public Methods

        public void Pause()
        {
            if (!IsPausing)
            {
                IsPausing = true;
                OnPause?.Invoke();
            }
            else
            {
                IsPausing = false;
                OnResume?.Invoke();
            }
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

        #endregion
    }
}
