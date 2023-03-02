using UnityEngine;
using UnityEditor;
using System.Collections;

namespace TemplateEditor.Processor
{
    /// <summary>
    /// �A�j���[�V�����N���b�v���C���|�[�g�������ɐݒ��ύX����
    /// </summary>
    public class AnimationPostprocessor : AssetPostprocessor
    {
        #region Unity Methods

        private void OnPreprocessAnimation()
        {
            var importer = assetImporter as ModelImporter;

            //����C���|�[�g�݂̂ɐ���
            if (!importer.importSettingsMissing) return;

            SetModelImporter(importer);
        }

        #endregion

        #region Private Methods

        private void SetModelImporter(ModelImporter importer)
        {
            var clips = importer.clipAnimations;

            if (clips.Length == 0) clips = importer.defaultClipAnimations;

            foreach (var clip in clips) clip.loopTime = true;

            importer.clipAnimations = clips;
        }

        #endregion
    }
}