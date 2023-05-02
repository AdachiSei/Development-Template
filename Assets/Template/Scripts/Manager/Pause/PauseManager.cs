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

        public event Action OnPause;
        public event Action OnResume;

        #endregion

        #region Unity Methods

        void Update()
        {
            if (Input.GetButtonDown(InputName.CANCEL))
            {
                if (!_isPausing)
                {
                    _isPausing = true;
                    OnPause();
                }
                else
                {
                    _isPausing = false;
                    OnResume();
                }
            }
        }

        #endregion
    }
}