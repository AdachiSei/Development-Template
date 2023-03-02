using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Template.Manager;
using Template.AudioData;

namespace TemplateEditor.Inspector
{
    /// <summary>
    /// サウンドマネージャーのエディター拡張
    /// </summary>
    [CustomEditor(typeof(SoundManager))]
    public class SoundManagerEditor : Editor
    {
        #region Member Variables

        private static bool _isOpening;
        private int _maxValue = 100;

        #endregion

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Editor", MonoScript.FromScriptableObject(this), typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();

            base.OnInspectorGUI();

            var soundManager = target as SoundManager;
            var style = new GUIStyle(EditorStyles.label);
            style.richText = true;

            EditorGUILayout.Space();

            _isOpening = EditorGUILayout.Foldout(_isOpening, "Settings");
            if (_isOpening)
            {
                EditorGUILayout.Space();

                //全フォルダから音をとってくる
                EditorGUILayout
                    .LabelField("<b>全フォルダから音をとってくる</b>", style);
                {
                    if (GUILayout.Button("GetAudioClips")) GetAudioDatas();
                }

                EditorGUILayout.Space();

                //BGM用のPrefabを作成
                EditorGUILayout
                    .LabelField("<b>音楽のオーディオソースを生成</b>", style);
                {
                    if (GUILayout.Button("CreateBGM")) soundManager.CreateBGM();
                }

                EditorGUILayout.Space();

                EditorGUILayout
                    .LabelField("<b>効果音のオーディオソースを生成</b>", style);
                {
                    var intField =
                        EditorGUILayout
                            .IntField
                                ("生成数", soundManager.AudioSourceCount);

                    var lessThanZero = intField < 0;
                    var overHundred = intField > _maxValue;
                    if (lessThanZero) intField = 0;
                    else if (overHundred) intField = _maxValue;

                    soundManager.SetAudioCount(intField);

                    if (GUILayout.Button("CreateSFX"))
                    {
                        soundManager.CreateSFX();
                    }
                }

                EditorGUILayout.Space();

                EditorGUILayout
                    .LabelField("<b>音楽&効果音のオーディオソースを全削除</b>", style);
                {
                    EditorGUI.BeginDisabledGroup(soundManager.IsStopingToCreate);

                    if (GUILayout.Button("Init"))
                    {
                        soundManager.Init();
                    }

                    EditorGUI.EndDisabledGroup();
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 全ての音のデータをとってくる関数
        /// </summary>
        private void GetAudioDatas()
        {
            var soundManager = target as SoundManager;

            var bgmList = new List<BGMData>();
            var sfxList = new List<SFXData>();

            foreach (var guid in AssetDatabase.FindAssets("t:BGMData"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var pathName = AssetDatabase.LoadMainAssetAtPath(path);
                var data = pathName as BGMData;
                bgmList.Add(data);
            }

            foreach (var guid in AssetDatabase.FindAssets("t:SFXData"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var pathName = AssetDatabase.LoadMainAssetAtPath(path);
                var data = pathName as SFXData;
                sfxList.Add(data);
            }

            soundManager.ResizeBGMClips(bgmList.Count);
            soundManager.ResizeSFXClips(sfxList.Count);

            for (int i = 0; i < bgmList.Count; i++)
                soundManager.AddBGMClip(i, bgmList[i]);

            for (int i = 0; i < sfxList.Count; i++)
                soundManager.AddSFXClip(i, sfxList[i]);
        }

        #endregion
    }
}