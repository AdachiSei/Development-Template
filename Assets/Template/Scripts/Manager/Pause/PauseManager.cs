using Template.Constant;
using UnityEngine;

namespace Template.Manager
{
    /// <summary>
    /// ポーズマネージャー
    /// </summary>
    public class PauseManager : MonoBehaviour
    {
        #region Properties

        public PauseData PauseData { get; private set; } = new();

        #endregion

        #region Unity Methods

        void Update()
        {
            if (Input.GetButtonDown(InputName.CANCEL))
                PauseData.Pause();
        }

        #endregion
    }
}
