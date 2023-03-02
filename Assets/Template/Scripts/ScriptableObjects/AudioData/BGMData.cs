using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音楽用のスクリプタブルオブジェクト
/// </summary>
[CreateAssetMenu(fileName = "BGMData", menuName = "ScriptableObjects/BGMData", order = 0)]
public class BGMData : ScriptableObject
{
    #region Properties

    public string Name => _name;
    public int Volume => _volume;
    public AudioClip BGMClip => _bgmClip;

    #endregion

    #region Inspector Variables

    [SerializeField]
    [Header("名前")]
    private string _name;

    [SerializeField]
    [Header("音量")]
    private int _volume = 1;

    [SerializeField]
    [Header("音楽")]
    private AudioClip _bgmClip;

    #endregion
}