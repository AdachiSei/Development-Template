using System;
using System.Collections;
using System.Collections.Generic;
using Template.Constant;
using UnityEngine;

namespace Template.Manager
{
    /// <summary>
    /// ポーズマネージャー
    /// </summary>
    public class PauseManager : MonoBehaviour
    {
        #region Member Variables

        private bool _isPausing = false;

        #endregion

        #region Events

        private event Action OnPause;
        private event Action OnResume;

        #endregion

        #region Unity Methods

        void Update()
        {
            if (Input.GetButtonDown(InputName.CANCEL))
            {
                if (!_isPausing)
                {
                    _isPausing = true;
                    OnPause?.Invoke();
                }
                else
                {
                    _isPausing = false;
                    OnResume?.Invoke();
                }
            }
        }

        #endregion

        #region Public Methods

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
