using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace TemplateEditor.Processor
{
    /// <summary>
    /// オーディオクリップをインポートした時に設定を変更する
    /// https://amagamina.jp/blog/audio-clip/
    /// https://qiita.com/ptkyoku/items/84b62cfbb4282a7cd7e6
    /// https://kan-kikuchi.hatenablog.com/entry/AudioSettings
    /// </summary>
    public class AudioPostprocessor : AssetPostprocessor
    {
        #region Constants

        private const float BGM_LENGTH = 10;

        #endregion

        #region Unity Method

        private void OnPostprocessAudio(AudioClip clip)
        {
            var importer = assetImporter as AudioImporter;

            //初回インポートのみに制限
            if (!importer.importSettingsMissing) return;

            SetAudioImporter(clip, importer);
        }

        #endregion

        #region Private Method

        /// <summary>
        /// 音楽や効果音を適した設定に変更する関数
        /// </summary>
        private void SetAudioImporter(AudioClip clip, AudioImporter importer)
        {
            //DefaultのAudioImporterSampleSettings取得
            var settings = importer.defaultSampleSettings;

            var isBGM = clip.length >= BGM_LENGTH;
            if (isBGM)//音楽
            {
                //ロードしながら再生を行うので、メモリをほんの少ししか使わない
                settings.loadType = AudioClipLoadType.Streaming;
            }
            else//効果音
            {
                //ファイルサイズがおよそ半分になる
                importer.forceToMono = true;
                //展開速度が早い、CPUの負荷を抑えられる
                settings.compressionFormat = AudioCompressionFormat.ADPCM;
            }
            //変更を反映
            importer.defaultSampleSettings = settings;
        }

        #endregion
    }
}