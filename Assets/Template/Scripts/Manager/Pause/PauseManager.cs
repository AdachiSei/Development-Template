using Template.Constant;
using UnityEngine;
using Zenject;

namespace Template.Pause
{
    /// <summary>
    /// ポーズマネージャー
    /// </summary>
    public class PauseManager : MonoBehaviour
    {
        [Inject]
        private IPauseable _pauseable = null;

        void Update()
        {
            if (Input.GetButtonDown(InputName.CANCEL))
                _pauseable.Pause();
        }
    }
}
