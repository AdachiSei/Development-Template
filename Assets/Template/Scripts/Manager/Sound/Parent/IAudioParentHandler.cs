using UnityEngine;
/// <summary>
/// オーディオソースを格納する親オブジェクトを管理するインターフェース
/// </summary>
public interface IAudioParentHandler
{
    Transform BGM { get; }
    Transform SFX { get; }
}
