using System;
using UnityEngine;

/// <summary>
/// オーディオソースを格納する親オブジェクト
/// </summary>
public class AudioParentHandler : IAudioParentHandler
{
    /// <summary>
    /// 音楽用のを格納する親オブジェクト
    /// </summary>
    public Transform BGM { get; private set; }
    /// <summary>
    /// 効果音用のを格納する親オブジェクト
    /// </summary>
    public Transform SFX { get; private set; }

    public AudioParentHandler(Transform bgmParent, Transform sfxParent)
    {
        BGM = bgmParent;
        SFX = sfxParent;
    }
}