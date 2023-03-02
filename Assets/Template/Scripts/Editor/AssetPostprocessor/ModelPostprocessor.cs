using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TemplateEditor.Processor
{
    /// <summary>
    /// モデルをインポートした時に設定を変更する
    /// </summary>
    public class ModelPostprocessor : AssetPostprocessor
    {
        #region Unity Methods

        private void OnPostprocessModel(GameObject go)
        {
            var importer = assetImporter as ModelImporter;

            //初回インポートのみに制限
            if (!importer.importSettingsMissing) return;

            SetModelImpoter(go, importer);
        }

        #endregion

        #region Private Methods

        private void SetModelImpoter(GameObject go, ModelImporter importer)
        {
            //ヒューマン用に設定
            importer.animationType = ModelImporterAnimationType.Human;
            if (go.name.Contains("@"))
            {
                importer.avatarSetup = ModelImporterAvatarSetup.CopyFromOther;
                var modelName = go.name.Split('@');
                var avatar = GameObject
                                .Find(modelName[0])
                                .GetComponent<Animator>()
                                .avatar;
                importer.sourceAvatar = avatar;
            }
        }

        #endregion
    }
}