using System;
using System.Collections;
using System.Collections.Generic;
using Template.Manager;

/// <summary>
/// サウンドマネージャー用のデータ
/// </summary>
public static class SoundManagerData
{
    public static int AudioSourceCount { get; private set; } = 10;

    /// <summary>
    /// 生成するSFX用Audioの数を変更する関数
    /// </summary>
    /// <param name="count">生成するAudioの数</param>
    public static void SetAudioSourceCount(int count)
    {
        AudioSourceCount = count;
    }
}