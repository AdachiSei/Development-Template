using UnityEngine;
using System;

namespace Template.Singleton
{
    /// <summary>
    /// シングルトンパターン
    /// </summary>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Properties

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Type t = typeof(T);

                    instance = (T)FindObjectOfType(t);
                    if (instance == null)
                    {
                        Debug.LogWarning($"{t}をアタッチしているオブジェクトがありません");
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Member Variables

        private static T instance;

        #endregion

        #region Unity Methods

        virtual protected void Awake()
        {
            // 他のゲームオブジェクトにアタッチされているか調べる
            // アタッチされている場合は破棄する。
            CheckInstance();
        }

        #endregion

        #region Private Methods

        protected bool CheckInstance()
        {
            if (instance == null)
            {
                instance = this as T;
                return true;
            }
            else if (Instance == this)
            {
                return true;
            }
            Destroy(gameObject);
            return false;
        }

        #endregion
    }
}