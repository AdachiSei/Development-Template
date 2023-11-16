/// <summary>
/// 音量を管理しているクラス
/// </summary>
public class VolumeData : IVolumeData
{
    public float Master { get; set; } = 1;
    public float BGM { get; set; } = 1;
    public float SFX { get; set; } = 1;
    public float MasterBGM => Master * BGM;
    public float MasterSFX => Master * SFX;

    public VolumeData(float masterVolume, float bgmVolume, float sfxVolume)
    {
        Master = masterVolume;
        BGM = bgmVolume;
        SFX = sfxVolume;
    }

    public void SetMasterVolume(float volume)
    {
        Master = volume;
    }

    public void SetBGMVolume(float volume)
    {
        BGM = volume;
    }

    public void SetSFXVolume(float volume)
    {
        SFX = volume;
    }
}