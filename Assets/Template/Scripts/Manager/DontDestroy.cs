using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    #region Unity Methods

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    #endregion
}