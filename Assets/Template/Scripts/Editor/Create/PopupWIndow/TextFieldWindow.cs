using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TemplateEditor.Window
{
    /// <summary>
    /// 文字を入力するためのポップアップウィンドウ
    /// </summary>
    public class TextFieldWindow : PopupWindowContent
    {
        #region Properties

        public string _scriptName = "NewScript";

        #endregion

        #region Events

        public event Action<string> OnCreate = null;

        #endregion

        #region Init Methods

        public TextFieldWindow(Action<string> action)
        {
            OnCreate = null;
            OnCreate += action;
        }

        #endregion

        #region Unity Methods

        /// <summary>
        /// サイズを取得する
        /// </summary>
        public override Vector2 GetWindowSize()
        {
            return new(200f, 50f);
        }

        /// <summary>
        /// GUI描画
        /// </summary>
        public override void OnGUI(Rect rect)
        {
            var style = new GUIStyle(EditorStyles.label);
            style.richText = true;

            EditorGUILayout.LabelField("<b>Please Enter The Script Name</b>", style);

            var scriptName = 
                EditorGUILayout.DelayedTextField(_scriptName);
            _scriptName = scriptName;

            if (_scriptName != "NewScript") OnClose();
        }

        /// <summary>
        /// 閉じたときに呼ばれる関数
        /// </summary>
        public override void OnClose()
        {
            base.OnClose();
            OnCreate?.Invoke(_scriptName);
        }

        #endregion
    }
}