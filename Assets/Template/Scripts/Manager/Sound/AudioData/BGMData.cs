using UnityEngine;

namespace Template.AudioData
{
    /// <summary>
    /// 音楽用のスクリプタブルオブジェクト
    /// </summary>
    [CreateAssetMenu(fileName = "BGMData", menuName = "ScriptableObjects/BGMData", order = 0)]
    public class BGMData : AudioDataBase
    {
        public AudioSource Source { get; private set; }

        public void SetBGMSource(AudioSource audioSource)
        {
            Source = audioSource;
        }
    }
}