using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Template.AudioData
{
    /// <summary>
    /// スクリプタブルオブジェクト
    /// </summary>
    public abstract class AudioData : ScriptableObject
    {
        #region Properties

        public string NickName => _nickName;
        public AudioClip AudioClip => _clip;

        #endregion

        #region Inspector Variables

        [SerializeField]
        [Header("名前")]
        private string _nickName;

        [SerializeField]
        [Header("効果音")]
        private AudioClip _clip;

        #endregion
    }
}