/// <summary>
/// 音量を表すインターフェース
/// </summary>
public interface IVolumeData
{
    float Master { get; }
    float BGM { get; }
    float SFX { get; }
    float MasterBGM { get; }
    float MasterSFX { get; }

    void SetMasterVolume(float volume);
    void SetBGMVolume(float volume);
    void SetSFXVolume(float volume);
}
